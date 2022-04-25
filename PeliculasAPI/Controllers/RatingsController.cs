using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public RatingsController(UserManager<IdentityUser> userManager
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDTO ratingDTO)
        {

            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;//valor email
            var usuario = await userManager.FindByNameAsync(email);
            var usuarioId = usuario.Id;

            var ratingActual = await context.Ratings
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingDTO.PeliculaId
                 && x.UsuarioId == usuarioId);

            if(ratingActual == null)
            {
                var rating = new Rating();
                rating.PeliculaId = ratingDTO.PeliculaId;
                rating.Puntuacion = ratingDTO.Puntuacion;
                rating.UsuarioId = usuarioId;
                context.Add(rating);
               
            }
            else//en el caso que el usuario votó anteriormente por la pelicula
            {
                ratingActual.Puntuacion=ratingDTO.Puntuacion;
                
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
