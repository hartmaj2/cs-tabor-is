using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Shared;

// This represents the participant data needed for the client side
// The difference is, that this participant doesn't have Id property because that is not filled in the form by the user
public class ParticipantFormData
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [RegularExpression(@"^([A-Z][A-Za-z]*(\. |[\.\- ])?)+$", ErrorMessage = "First name must start with capital letter and contain no digits.")]
    public string? FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    [RegularExpression(@"^([A-Z][A-Za-z]*(\. |[\.\- ])?)+$", ErrorMessage = "First name must start with capital letter and contain no digits.")]
    public string? LastName {get; set;}

    public const int LowestAge = 0;
    public const int HighestAge = 130;
    [Required(ErrorMessage = "Age is required.")]
    [Range(LowestAge, HighestAge, ErrorMessage = "Age must be between 0 and 130.")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "The phone number must consist of exactly 9 digits.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Birth number is required.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Birth number must consist of exaclty 10 digits.")]
    [DivisibleBy(11)]
    public string? BirthNumber { get; set; }

    public IList<AllergenSelection>? DietSelections;

    public ParticipantDto ConvertToParticipantDto()
    {
        return new ParticipantDto
        {
            Id = Id,
            FirstName = FirstName!,
            LastName = LastName!,
            Age = Age,
            PhoneNumber = PhoneNumber,
            BirthNumber = BirthNumber!,
            // Add a corresponding AllergenDto only when the selection IsSelected
            Diets = DietSelections!.Where(selection => selection.IsSelected).Select(selection => new AllergenDto {Name = selection.Name}).ToList()
        };
    }

    public ParticipantDto ConvertToParticipantDto(int id)
    {
        var participant = ConvertToParticipantDto();
        participant.Id = id;
        return participant;
    }

}

// Class used for custom validation of divisivility
public class DivisibleByAttribute : ValidationAttribute
{

    private long _divisor;
    public DivisibleByAttribute(long divisor)
    {
        _divisor = divisor;
    }

    // Here for some reason the form was not validating properly when using ValidationResult overload of the IsValid method
    // If I returned new ValidationResult with an error message, the form still showed the box as green even though the error message was displayed
    // That is the reason I am using bool IsValid override instead
    public override bool IsValid(object? value)
    {
        if (value is string stringValue && long.TryParse(stringValue,out long number) && number % _divisor == 0)
        {
            return true;
        }
        ErrorMessage = $"Birth number must be divisible by {_divisor}";
        return false;
    }
}

// Used by EditParticipant razor component after it receives the api participant from the database
public static class ParticipantDtoExtensions
{
    public static ParticipantFormData ConvertToParticipantFormData(this ParticipantDto participant, IEnumerable<AllergenDto> allAllergens)
    {
        return new ParticipantFormData
        {
            Id = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            Age = participant.Age,
            PhoneNumber = participant.PhoneNumber,
            BirthNumber = participant.BirthNumber,

            // Go through all possible allergens and create a new allergen for each one with corresponding name and marked as selected if the name of the allergen is contained in the participantDto diets
            DietSelections = 
                allAllergens
                    .Select( allergen => new AllergenSelection()
                        { 
                            Name = allergen.Name, 
                            IsSelected = participant.Diets.Select(diet => diet.Name).Contains(allergen.Name)
                        })
                    .ToList()
        };
    }
}