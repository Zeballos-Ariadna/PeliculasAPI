using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades
{
    public class Genero: IValidatableObject
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="El campo {0} es requerido")]//El campo nombre es obligatorio
        [StringLength(maximumLength: 10)]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Range(18,110)]
        public int Edad { get; set; }

        [CreditCard]//Valida la tarjeta usando mod 10
        public string TarjetaDeCredito { get; set; }

        [Url]
        public string Url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))//si no es nulo o vacío
            {
                var primeraLetra = Nombre[0].ToString();
                if(primeraLetra != primeraLetra.ToUpper())//si la letra es != a la letra en mayus
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula",
                        new string[] { nameof(Nombre) });//el error le corresponde al campo Nombre
                }
            }
        }
    }
}
