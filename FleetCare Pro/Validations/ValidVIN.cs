using System.ComponentModel.DataAnnotations;

namespace FleetCare_Pro.Validations
{
    public class ValidVIN: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("VIN is required.");
            }

            string vin = value.ToString().ToUpper(); 

            if (vin.Length != 17)
            {
                return new ValidationResult("VIN must be exactly 17 characters long.");
            }

            if (vin.Contains("I") || vin.Contains("O") || vin.Contains("Q"))
            {
                return new ValidationResult("VIN cannot contain the letters I, O, or Q.");
            }

            return ValidationResult.Success;
        }
    }
}
