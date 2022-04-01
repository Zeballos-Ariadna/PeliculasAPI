using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="El campo {0} es requerido")]//El campo nombre es obligatorio
        [StringLength(maximumLength: 10)]
        public string Nombre { get; set; }

        [Range(18,110)]
        public int Edad { get; set; }

        [CreditCard]
        public string TarjetaDeCredito { get; set; }

        [Url]
        public string Url { get; set; }
    }
}
