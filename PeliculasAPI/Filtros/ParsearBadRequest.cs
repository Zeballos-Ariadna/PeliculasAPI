using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PeliculasAPI.Filtros
{
    public class ParsearBadRequest : IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            var casteoResult = context.Result as IStatusCodeActionResult;
            if(casteoResult == null)
            {
                return;
            }
            
            var codigoEstatus = casteoResult.StatusCode;
            if(codigoEstatus == 400)
            {
                var respuesta = new List<string>();
                var resultadoActual = context.Result as BadRequestObjectResult;
                if(resultadoActual.Value is string)
                {
                    respuesta.Add(resultadoActual.Value.ToString());
                }
                else if(resultadoActual.Value is IEnumerable<IdentityError> errores)
                {
                    foreach(var error in errores)
                    {
                        respuesta.Add(error.Description);
                    }
                }
                else
                {
                    foreach (var llave in context.ModelState.Keys)
                    {
                        foreach (var error in context.ModelState[llave].Errors)
                        {
                            respuesta.Add($"{llave}: {error.ErrorMessage}");
                        }
                    }
                }
                //Se genera un listado de strings con errores y/o strings
                //es más fácil de procesar en frontend
                context.Result = new BadRequestObjectResult(respuesta); 
            }
            

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
