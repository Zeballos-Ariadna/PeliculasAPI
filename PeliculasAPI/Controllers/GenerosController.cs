using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Entidades;
using PeliculasAPI.Repositorios;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]//Endpoint
    public class GenerosController
    {
        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]
        public List<Genero> Get()
        {
            return repositorio.ObtenerTodosLosGeneros();
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Genero> Get(int Id)
        {
            var _genero = repositorio.ObtenerPorId(Id);

            if (_genero == null) return new NotFoundResult();

            return _genero;
        }

        [HttpPost]
        public void Post()
        {

        }
        [HttpPut]
        public void Put()
        {

        }
        [HttpDelete]
        public void Delete()
        {

        }
    }
}
