// Participant model for communication of participant diets between Client and Server
// I want to communicate allergens as list of Allergens

public class ParticipantDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int Age => GetAge();

    public string? PhoneNumber {get; set; }

    public required string BirthNumber { get; set; }

    public required List<AllergenDto> Diets { get; set; }

    private int GetAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var birthDate = GetBirthNumberDateOnly();
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month) age --; // it wasn't my birthday this year yet
        else if (today.Month == birthDate.Month && today.Day < birthDate.Day) age--; // it wasn't my birthday this year yet
        return age;
        
    }

    // get the date only from the birth number
    private DateOnly GetBirthNumberDateOnly()
    {
        int year = ParseYear(BirthNumber[0..2]);
        int month = ParseMonth(BirthNumber[2..4]);
        int day = int.Parse(BirthNumber[4..6]);
        return new DateOnly(year,month,day);
    }

    // convert two last digits from year, this assumes that the person was not born before year 54
    private static int ParseYear(string twoLastDigits)
    {
        var year = int.Parse(twoLastDigits);
        if (year >= 54)
        {
            return year + 1900;
        } 
        return year += 2000;
    }

    // convert month from birth number according to czech rules
    private static int ParseMonth(string monthString)
    {
        var month = int.Parse(monthString);
        if (month >= 50) month -= 50;
        if (month >= 20) month -= 20;
        return month;     
    }

}

public static class ParticipantExtensions
{
    // Converts a Participant to Participant diets dto, the navigation properties ParticipantAllergens and Allergen must be loaded from db explicitly using Include
    public static ParticipantDto ConvertToParticipantDto(this Participant thisParticipant)
    {
        return new ParticipantDto()
        {
            Id = thisParticipant.Id,
            FirstName = thisParticipant.FirstName,
            LastName = thisParticipant.LastName,
            PhoneNumber = thisParticipant.PhoneNumber,
            BirthNumber = thisParticipant.BirthNumber,
            Diets = thisParticipant.ParticipantAllergens!.Select(pa => pa.Allergen!.ToAllergenDto()).ToList()
        };
    }
}

