using System.ComponentModel.DataAnnotations;

// This represents the participant data needed for the client side
// Most of the properties also have backing fields and custom setters
// The important thing here is that the validation logic caused by data annotation is processed AFTER the setters are processed
public class ParticipantFormData
{

    // 
    // ID
    // 

    public int Id { get; set; } // Id is included even though we cannot set it through the form because we don't want to lose id when converting to ParticipantDto and back

    // 
    // FIRST NAME
    // 

    private string _firstName = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [ValidName("first name")]
    public required string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value.Trim(); // This removes leading or trailing white spaces when user enters them to the form (after the user presses enter)
        }
    }

    // 
    // LAST NAME
    // 

    private string _lastName = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [ValidName("last name")]
    public required string LastName 
    {
        get => _lastName;
        set
        {
            _lastName = value.Trim(); // This removes leading or trailing white spaces when user enters them to the form (after user presses enter)
        }
    }

    // 
    // AGE
    // 

    public const int LowestAge = 0;
    public const int HighestAge = 70; // I want to support only birth numbers later than 1954 that have the 10 digit format

    [Required(ErrorMessage = "The age is required")]
    [IntegerRangeValidator(LowestAge,HighestAge,nameof(Age))]
    public int? Age { get; set; } // the age is set automatically if the entered birth number is valid

    // 
    // PHONE NUMBER
    // 

    private static readonly string[] ValidPrefixes = ["+420","00420"]; // czech prefixes that are considered valid phone number inputs
    private static readonly string[] CharactersToRemove = [" ",".","-","(",")","[","]"]; // these characters add no meaning to the phone number so we can just remove them 

    private string _phoneNumber = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [ValidPhoneNumber]
    public required string PhoneNumber
    { 
        get => _phoneNumber;
        set
        {
            if (value == string.Empty) return; // if nothing was entered, don't continue setting the phone number based on custom rules
            _phoneNumber = value.Trim();
            foreach (var meaningless in CharactersToRemove)
            {
                _phoneNumber = _phoneNumber.Replace(meaningless,""); // remove characters that add no meaning from the phone number string
            }
            foreach (var prefix in ValidPrefixes) // if the number contains a valid prefix, remove the prefix and leave just the rest
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

    [ValidBirthNumber]
    public required string BirthNumber 
    { 
        get => _birthNumber;
        set 
        {
            if (value == string.Empty) return;
            var stringValue = value.Trim();
            if (stringValue.Length == 11 && stringValue[6] == '/') // if user entered the birth number with / character
            {
    
                _birthNumber = stringValue[..6] + stringValue[7..];
            }
            else
            {
                _birthNumber = stringValue;
            }
            if (ValidBirthNumberAttribute.Instance.IsValid(_birthNumber)) // if the birth number is valid, set the age automatically
            {
                Console.WriteLine("setting age");
                Age = BirthNumberToAgeParser.Parse(_birthNumber);
            }            
        } 
    
    }

    public IList<AllergenSelection>? DietSelections;

    // Used after submit to pass the new details to api (api needs the participant as ParticipantDto)
    public ParticipantDto ToParticipantDto()
    {
        return new ParticipantDto
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = PhoneNumber,
            BirthNumber = BirthNumber,
            Age = (int) Age!, // here I know the form will not allow me to submit without having the age set to something

            // Add a corresponding AllergenDto only when the selection IsSelected
            Diets = DietSelections!.Where(selection => selection.IsSelected).Select(selection => new AllergenDto {Name = selection.Name}).ToList()
        };
    }

    // Factory method to create ParticipantFormData with all values empty
    public static ParticipantFormData CreateDefault()
    {
        return new ParticipantFormData() {FirstName = string.Empty, BirthNumber = string.Empty, PhoneNumber = string.Empty ,LastName = string.Empty};
    }

}

// Parses Czech birth numbers in 10-digit format used after 1.1.1954 to the corresponding age
public static class BirthNumberToAgeParser
{

    // Needs to be called with validBirthNumber!
    public static int Parse(string validBirthNumber)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var birthDate = GetBirthNumberDateOnly(validBirthNumber);
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month) age --; // it wasn't my birthday this year yet
        else if (today.Month == birthDate.Month && today.Day < birthDate.Day) age--; // it wasn't my birthday this year yet
        return age;
    }

    // Get the DateOnly object that corresponds to the given birth number string
    private static DateOnly GetBirthNumberDateOnly(string validBirthNumber)
    {
        int year = ParseYear(validBirthNumber[0..2]);
        int month = ParseMonth(validBirthNumber[2..4]);
        int day = int.Parse(validBirthNumber[4..6]);
        return new DateOnly(year,month,day);
    }

    // Convert two last digits from year, this assumes that the person was not born before year 54
    private static int ParseYear(string twoLastDigits)
    {
        var year = int.Parse(twoLastDigits);
        if (year >= 54)
        {
            return year + 1900;
        } 
        return year += 2000;
    }

    // Convert month from birth number according to czech birth number format rules
    // For girls 50 or 70 is added to the birth number
    // For boys 0 or 20 is added to the birth number
    private static int ParseMonth(string monthString)
    {
        var month = int.Parse(monthString);
        if (month >= 50) month -= 50;
        if (month >= 20) month -= 20;
        return month;     
    }
}

public static class ParticipantDtoExtensions
{   

    // Used to pass data to edit participant modal to convert from ParticipantDto received from api
    public static ParticipantFormData ToParticipantFormData(this ParticipantDto participant, IEnumerable<AllergenDto> allAllergens)
    {
        return new ParticipantFormData
        {
            Id = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            PhoneNumber = participant.PhoneNumber,
            BirthNumber = participant.BirthNumber,
            Age = participant.Age,

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