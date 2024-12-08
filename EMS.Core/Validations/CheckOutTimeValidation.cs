using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Validations;

public class CheckOutTimeValidation : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public CheckOutTimeValidation(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var currentValue = (TimeSpan?)value;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = (TimeSpan?)property.GetValue(validationContext.ObjectInstance);

        if (currentValue <= comparisonValue)
            return new ValidationResult($"Check-out time must be later than check-in time.");

        return ValidationResult.Success;
    }
}
