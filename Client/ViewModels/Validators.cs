using System.ComponentModel.DataAnnotations;

// Class used for custom validation of divisivility
public class DivisibleByElevenAttribute : ValidationAttribute
{

    // Here for some reason the form was not validating properly when using ValidationResult overload of the IsValid method
    // If I returned new ValidationResult with an error message, the form still showed the box as green even though the error message was displayed
    // That is the reason I am using bool IsValid override instead
    public override bool IsValid(object? value)
    {
        if (value is string stringValue)
        {
            int checkSum = 0;
            for (int i = 0; i < stringValue.Length; i++)
            {
                var currentChar = stringValue[i];
                if (!char.IsDigit(currentChar))
                {
                    ErrorMessage = $"The birth number must contain only digits";
                    return false;
                }
                var multiple = (i % 2 == 0) ? 1 : -1;
                checkSum += multiple * (currentChar - '0');
            }
            if (checkSum % 11 == 0)
            {
                return true;
            }
            
        }
        ErrorMessage = $"Birth number must be divisible by 11";
        return false;
    }
}