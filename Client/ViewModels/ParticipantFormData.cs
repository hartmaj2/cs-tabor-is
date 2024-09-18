using System.ComponentModel.DataAnnotations;

// This represents the participant data needed for the client side
// The difference is, that this participant doesn't have Id property because that is not filled in the form by the user
public class ParticipantFormData
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [ValidName("first name")]
    public string? FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    [ValidName("last name")]
    public string? LastName {get; set;}

    public const int LowestAge = 0;
    public const int HighestAge = 80; // I want to support only birth numbers later than 1954 that have the 10 digit format

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^((\+420)?\d{9})|(\+(?!420)\d{8,12})$", ErrorMessage = "The phone number is not valid.")]
    public string? PhoneNumber { get; set; }

    private string _birthNumber = string.Empty;

    [Required(ErrorMessage = "Birth number is required.")]
    [ValidBirthNumber]
    public string? BirthNumber 
    { 
        get
        {
            return _birthNumber;
        }
        set // the validation logic cause by data annotation is processed AFTER the setter
        {
            if (value!.Length == 11 && value[6] == '/') // if user entered the birth number with / character
            {
     
                _birthNumber = value[..6] + value[7..];
            }
            else
            {
                _birthNumber = value;
            }
        } 
    
    }


    public IList<AllergenSelection>? DietSelections;

    public ParticipantDto ConvertToParticipantDto()
    {
        return new ParticipantDto
        {
            Id = Id,
            FirstName = FirstName!,
            LastName = LastName!,
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