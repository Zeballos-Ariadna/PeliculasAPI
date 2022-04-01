using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeliculasAPI.Entidades;
using PeliculasAPI.Repositorios;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]//Endpoint
    [ApiController]//Devuelve error si el modelo de la acción es inválida, indicando las razones del error
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;

        public GenerosController(IRepositorio repositorio, WeatherForecastController weatherForecastController)
        {
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
        }

        [HttpGet]
        public ActionResult<List<Genero>> Get()
        {
            return repositorio.ObtenerTodosLosGeneros();
        }

        [HttpGet("guid")]//api/generos/guid
        public ActionResult<Guid> GetGUID()
        {
            return Ok(new 
                {
                    GUID_GenerosController = repositorio.ObtenerGUID(),
                    GUID_WeatherForecastController= weatherForecastController.ObtenerGUIDWeatherForecastController()
                });
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genero>> Get(int Id, [FromHeader] string nombre)
        {
            
            var _genero = await repositorio.ObtenerPorId(Id);

            if (_genero == null)
            {
                return new NotFoundResult();//Retorna 404(No encontrada)
            }

            return _genero;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero)//FromBody importante para cuando trabajamos con la creación y actualizacion de registros
        {
            repositorio.CrearGenero(genero);
            return NoContent();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            return NoContent();
        }
        [HttpDelete]
        public void Delete()
        {

        }
    }
}
