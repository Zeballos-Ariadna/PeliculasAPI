﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace PeliculasAPI.Filtros
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> logger;

        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);//Procesa cualquier error que no haya sido atrapado por un try catch
            base.OnException(context);
        }

    }
}
