using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI;
using PeliculasAPI.ApiBehavior;
using PeliculasAPI.Controllers;
using PeliculasAPI.Filtros;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//Transient es el tiempo más corto de vida que le podemos dar a un Servicio

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddResponseCaching();//permite acceso a los servic del sistema por defecto de caching
//AddScoped es para tener configurado un Scope, el tiempo de vida de la clase instanciada
//va a ser durante toda la petición HTTP

//AddSingleton sirve para indicar que el tiempo de vida de la instancia del Sericio
//va a ser durante todo el tiempo de ejecución de la aplicación


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroDeExcepcion));
    options.Filters.Add(typeof(ParsearBadRequest));
}).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);

builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");//revisar si funciona
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});


//BD
var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


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
