using Microsoft.EntityFrameworkCore;

namespace PeliculasAPI.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext, 
            IQueryable<T> queryable)//HttpContext: coloca en cabecera la info de registros, IQueryable: devuelve la cant de registros en tabla
        {
            if(httpContext == null)
            {//
                throw new ArgumentNullException(nameof(httpContext));
            }

            double cantidad = await queryable.CountAsync();//cuenta la cant de registros de la tabla X
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}
