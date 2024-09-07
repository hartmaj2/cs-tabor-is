// Participant model for communication of participant diets between Client and Server
// I want to communicate allergens as list of Allergens

using Shared;

public class ParticipantDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public string? PhoneNumber {get; set; }

    public required string BirthNumber {get; set; }

    public required List<AllergenDto> Allergens { get; set; }

}

