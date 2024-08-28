using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;



public class ParticipantFormData
{


    [Required(ErrorMessage = "First name is required.")]
    public string? FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    public string? LastName {get; set;}

    [Required(ErrorMessage = "Age is required.")]
    [Range(0, 200, ErrorMessage = "Age must be between 0 and 200.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Birth number is required.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Birth number must contain only numbers.")]
    [DivisibleBy(11,ErrorMessage = "Birth number must be divisible by 11.")]
    public int? BirthNumber { get; set; }

}

public class DivisibleByAttribute : ValidationAttribute
{

    private int _divisor;
    public DivisibleByAttribute(int divisor)
    {
        _divisor = divisor;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int intValue && intValue % _divisor == 0)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}