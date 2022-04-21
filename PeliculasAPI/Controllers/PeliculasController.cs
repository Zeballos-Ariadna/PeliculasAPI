﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Utilidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        public PeliculasController(ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            //Listado de peliculas proximosEstrenos
            var proximosEstrenos = await context.Peliculas
                .Where(x => x.FechaLanzamiento > hoy)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            //Listado de peliculas EnCines
            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var resultado = new LandingPageDTO();
            resultado.ProximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .Include(x => x.PeliculasGeneros)//incluye PeliculasGeneros
                .ThenInclude(x => x.Genero)//incluye Genero que viene de PelGeneros
                .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                .FirstOrDefaultAsync(x => x.Id == id);
            //Incluye la data que viene en PeliculasGeneros 

            if (pelicula == null) { return NotFound(); }

            var dto = mapper.Map<PeliculaDTO>(pelicula);
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();
            return dto;
        }

        [HttpGet("PostGet")]//método Get que prepara la app para realizar un Post más adelante
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDTO>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO() { Cines = cinesDTO, Generos = generosDTO };
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if(peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var pelicula = peliculaActionResult.Value;

            //Busca los Ids de los generos seleccionados
            var generosSeleccionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))//Busco los generos que no tengan Id del genero selecionado
                .ToListAsync();
            //Busco los Ids de los cines seleccionados
            var cinesSeleccionadosIds = pelicula.Cines.Select(x => x.Id).ToList();
            var cinesNoSeleccionados = await context.Cines
                .Where(x => !cinesSeleccionadosIds.Contains(x.Id))//Busco los cines que no tengan Id del cine selecionado
                .ToListAsync();

            var generosNoSeleccionadosDTO = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);
            var cinesNoSeleccionadosDTO = mapper.Map<List<CineDTO>>(cinesNoSeleccionados);

            var respuesta = new PeliculasPutGetDTO();
            respuesta.Pelicula = pelicula;
            respuesta.GenerosSeleccionados = pelicula.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSeleccionadosDTO;
            respuesta.CinesSeleccionados = pelicula.Cines;
            respuesta.CinesNoSeleccionados = cinesNoSeleccionadosDTO;
            respuesta.Actores = pelicula.Actores;
            return respuesta;
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO) 
        {
            var peliculaQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltrarDTO.Titulo))
            {
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.Titulo.Contains(peliculasFiltrarDTO.Titulo));
            }

            if (peliculasFiltrarDTO.EnCines)
            {
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.EnCines);
            }

            if (peliculasFiltrarDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.FechaLanzamiento > hoy);
            }

            if (peliculasFiltrarDTO.GeneroId !=0)
            {
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.PeliculasGeneros.Select(y => y.GeneroId)//Navego por PeliculasGeneros que contiene GeneroId
                    .Contains(peliculasFiltrarDTO.GeneroId));//Trae las peliculas que contengan el GeneroId que estoy mandando
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculaQueryable);

            var peliculas = await peliculaQueryable.Paginar(peliculasFiltrarDTO.PaginacionDTO).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id,[FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .Include(x => x.PeliculasCines)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(pelicula == null)
            {
                return NotFound();
            }

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if(peliculaCreacionDTO.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos
                    .EditarArchivo(
                        contenedor, 
                        peliculaCreacionDTO.Poster, //archivo como tal que viene de ahí
                        pelicula.Poster);// ruta
            }

            EscribirOrdenActores(pelicula);
            await context.SaveChangesAsync();
            return NoContent();

        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if(pelicula.PeliculasActores != null)
            {
                for(int i= 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula != null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(pelicula.Poster, contenedor);
            return NoContent();


        }

    }
}
