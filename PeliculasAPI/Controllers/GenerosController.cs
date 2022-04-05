using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PeliculasAPI.Entidades;
using PeliculasAPI.Filtros;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]//Endpoint
    [ApiController]//Devuelve error si el modelo de la acción es inválida, indicando las razones del error
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//Protege los endpoints del controlador de géneros
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;

        public GenerosController(
            ILogger<GenerosController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]// api/generos
        public ActionResult<List<Genero>> Get()
        {
            return new List<Genero>()
            {
                new Genero() { Id = 1, Nombre = "Comedia"}
            };
        }

       

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genero>> Get(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero)//FromBody importante para cuando trabajamos con la creación y actualizacion de registros
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
