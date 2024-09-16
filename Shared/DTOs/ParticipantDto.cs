// Participant model for communication of participant diets between Client and Server
// I want to communicate allergens as list of Allergens

using Shared;

public class ParticipantDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int Age => GetAge();

    public string? PhoneNumber {get; set; }

    private string _birthNumber = string.Empty; // initialize to string.Empty to avoid nullability issues

    public required string BirthNumber
    { 
        get
        {
            return _birthNumber[..6] + "/" + _birthNumber[6..]; // when printing the birth number, add the / character
        }
        set
        {
            _birthNumber = value;
        }

    }

    public required List<AllergenDto> Diets { get; set; }

    private int GetAge()
    {
        return 14;
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

