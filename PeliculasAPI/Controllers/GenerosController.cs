using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeliculasAPI.Entidades;
using PeliculasAPI.Filtros;
using PeliculasAPI.Repositorios;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]//Endpoint
    [ApiController]//Devuelve error si el modelo de la acción es inválida, indicando las razones del error
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//Protege los endpoints del controlador de géneros
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;
        private readonly ILogger<GenerosController> logger;

        public GenerosController(IRepositorio repositorio, 
            WeatherForecastController weatherForecastController,
            ILogger<GenerosController> logger)
        {
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
            this.logger = logger;
        }

        [HttpGet]// api/generos
        //[ResponseCache(Duration =60)]// duración del caché activado
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult<List<Genero>> Get()
        {
            logger.LogInformation("Vamos a mostrar los géneros");
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
        public async Task<ActionResult<Genero>> Get(int Id)
        {
            logger.LogDebug($"Obteniendo un género por el id {Id}");
            
            var _genero = await repositorio.ObtenerPorId(Id);

            if (_genero == null)
            {
                logger.LogWarning($"No pudimos encontrar el génedo de id: {Id}");
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
