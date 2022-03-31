using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Entidades;
using PeliculasAPI.Repositorios;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]//Endpoint
    public class GenerosController: ControllerBase
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
        public async Task<ActionResult<Genero>> Get(int Id)
        {
            var _genero = await repositorio.ObtenerPorId(Id);

            if (_genero == null)
            {
                return new NotFoundResult();//Retorna 404(No encontrada)
            }

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
