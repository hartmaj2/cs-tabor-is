// Participant model for communication of participant diets between Client and Server
// I want to communicate allergens as list of Allergens

public class ParticipantDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int Age { get; set; }

    public required string PhoneNumber {get; set; }

    public required string BirthNumber { get; set; }

    public required List<AllergenDto> Diets { get; set; }



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
            Age = thisParticipant.Age,
            Diets = thisParticipant.ParticipantAllergens!.Select(pa => pa.Allergen!.ToAllergenDto()).ToList()
        };
    }
}

