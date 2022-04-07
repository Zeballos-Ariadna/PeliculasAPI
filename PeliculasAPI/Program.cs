using Microsoft.AspNetCore.Authentication.JwtBearer;
using PeliculasAPI.Controllers;
using PeliculasAPI.Filtros;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Transient es el tiempo m�s corto de vida que le podemos dar a un Servicio

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddResponseCaching();//permite acceso a los servic del sistema por defecto de caching
//AddScoped es para tener configurado un Scope, el tiempo de vida de la clase instanciada
//va a ser durante toda la petici�n HTTP

//AddSingleton sirve para indicar que el tiempo de vida de la instancia del Sericio
//va a ser durante todo el tiempo de ejecuci�n de la aplicaci�n


builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(FiltroDeExcepcion));    
});

builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");//revisar si funciona
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//Los Middlewares que comienzan con 'Use' son aquellos que no detienen el proceso
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
