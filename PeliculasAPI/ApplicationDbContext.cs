using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;

namespace PeliculasAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genero> Generos {get; set;}//Nombre de la tabla Generos
    
        public DbSet<Actor> Actores {get; set;}

        public DbSet<Cine> Cines { get; set; }
    }
}
