using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())){//Si el valor es vacío
                return ValidationResult.Success;
            }

            var primeraLetra= value.ToString()[0].ToString();//extraemos la 1era letra

            if (primeraLetra != primeraLetra.ToUpper()) //Si la 1era letra es distinta a la 1era letra convertida en mayúscula
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;
        }
    }
}
