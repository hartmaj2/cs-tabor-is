using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;

// This represents the participant data needed for the client side
// The difference is, that this participant doesn't have Id property because that is not filled in the form by the user
public class ParticipantFormData
{

    // 
    // ID
    // 

    public int Id { get; set; }

    // 
    // FIRST NAME
    // 

    private string _firstName = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [ValidName("first name")]
    public string? FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value!.Trim(); // This removes leading or trailing white spaces when user enters them to the form
        }
    }

    // 
    // LAST NAME
    // 

    private string _lastName = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [ValidName("last name")]
    public string? LastName 
    {
        get => _lastName;
        set
        {
            _lastName = value!.Trim(); // This removes leading or trailing white spaces when user enters them to the form
        }
    }

    // 
    // AGE
    // 

    public const int LowestAge = 0;
    public const int HighestAge = 80; // I want to support only birth numbers later than 1954 that have the 10 digit format

    // 
    // PHONE NUMBER
    // 

    private static readonly string[] ValidPrefixes = ["+420","00420"]; // czech prefixes that are considered valid phone number inputs
    private static readonly string[] CharactersToRemove = [" ",".","-","(",")","[","]"]; // these characters add no meaning to the phone number so we can just remove them 

    private string _phoneNumber = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^((\+420)?\d{9})|(\+(?!420)\d{8,12})$", ErrorMessage = "The phone number is not valid.")]
    public string? PhoneNumber
    { 
        get => _phoneNumber;
        set
        {
            _phoneNumber = value!.Trim();
            foreach (var meaningless in CharactersToRemove)
            {
                _phoneNumber = _phoneNumber.Replace(meaningless,""); // remove characters that add no meaning from the phone number string
            }
            foreach (var prefix in ValidPrefixes)
            {
                if (_phoneNumber[..prefix.Length].Equals(prefix))
                {
                    _phoneNumber = _phoneNumber[prefix.Length..];
                }
            }
        } 
    }

    // 
    // BIRTH NUMBER
    // 

    private string _birthNumber = string.Empty;

    [Required(ErrorMessage = "Birth number is required.")]
    [ValidBirthNumber]
    public string? BirthNumber 
    { 
        get => _birthNumber;
        set // the validation logic caused by data annotation is processed AFTER the setter
        {
            var stringValue = value!.Trim();
            if (stringValue!.Length == 11 && stringValue[6] == '/') // if user entered the birth number with / character
            {
    
                _birthNumber = stringValue[..6] + stringValue[7..];
            }
            else
            {
                _birthNumber = stringValue;
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