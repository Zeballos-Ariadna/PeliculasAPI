using Microsoft.AspNetCore.Mvc.Filters;

namespace PeliculasAPI.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Ejecutando antes de ejecutar la accion");
        }//Antes de ejecutar la accion

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Ejecutando despues de ejecutar la accion");
        }//Después de ejecutar la accion
    }
}
