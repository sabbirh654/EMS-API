using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Validations;

public class BirthDateValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime birthDate)
        {
            if (birthDate >= DateTime.Now)
            {
                return new ValidationResult(ErrorMessage ?? "Birth date must be in the past.");
            }
        }
        return ValidationResult.Success;
    }
}
