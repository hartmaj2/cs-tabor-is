using System.ComponentModel.DataAnnotations;

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
    [RegularExpression(@"^\+\d{12}$|^\d{9}$", ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Birth number is required.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Birth number must consist of exaclty 10 digits.")]
    [DivisibleBy(11)]
    public string? BirthNumber { get; set; }

}

// Class used for custom validation of divisivility
public class DivisibleByAttribute : ValidationAttribute
{

    private int _divisor;
    public DivisibleByAttribute(int divisor)
    {
        _divisor = divisor;
    }

    // Here for some reason the form was not validating properly when using ValidationResult overload of the IsValid method
    // If I returned new ValidationResult with an error message, the form still allowed a submit even though the error message was displayed
    // That is the reason I am using bool IsValid override instead
    public override bool IsValid(object? value)
    {
        if (value is string stringValue && int.TryParse(stringValue,out int number) && number % _divisor == 0)
        {
            return true;
        }
        ErrorMessage = $"Birth number must be divisible by {_divisor}";
        return false;
    }
}