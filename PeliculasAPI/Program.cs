using Microsoft.AspNetCore.Authentication.JwtBearer;
using PeliculasAPI.Controllers;
using PeliculasAPI.Filtros;
using PeliculasAPI.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Transient es el tiempo más corto de vida que le podemos dar a un Servicio

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddResponseCaching();//permite acceso a los servic del sistema por defecto de caching
//AddScoped es para tener configurado un Scope, el tiempo de vida de la clase instanciada
//va a ser durante toda la petición HTTP

//AddSingleton sirve para indicar que el tiempo de vida de la instancia del Sericio
//va a ser durante todo el tiempo de ejecución de la aplicación

builder.Services.AddScoped<IRepositorio, RepositorioEnMemoria>();
builder.Services.AddScoped<WeatherForecastController>();
builder.Services.AddTransient<MiFiltroDeAccion>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



/*app.Use(async (context, next) =>//Puede interceptar las peticiones Http y guardar en un log las respuestas de las mismas
{
    using(var swapStream = new MemoryStream())//Hacemos una copia para guardar en el log
    {
        //ILogger <Program> logger;//REVISAR!!
        var repuestaOriginal = context.Response.Body;
        context.Response.Body = swapStream;
        await next.Invoke();//Continua la ejecución del pipeline

        //En esta parte ya nos estan devolviendo una respuesta los Middlewares
        swapStream.Seek(0, SeekOrigin.Begin);//Para llevar al principio
        string respuesta=  new StreamReader(swapStream).ReadToEnd();//swapStream lo leemos hasta al final
        swapStream.Seek(0, SeekOrigin.Begin);//Volvemos a colocar al inicio

        await swapStream.CopyToAsync(repuestaOriginal);
        context.Response.Body = repuestaOriginal;
        //logger.LogInformation(respuesta); //REVISAR!!
    }
});

app.Map("/mapa1", (app) =>
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Estoy interceptando el pipeline");
    });
});*/



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//Los Middlewares que comienzan con 'Use' son aquellos que no detienen el proceso
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
