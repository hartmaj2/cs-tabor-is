// Participant model for communication of participant diets between Client and Server
// I don't need information like Age, PhoneNumber etc. 
// The reason to have separate class like this is so I don't transfer unnecessary data through json api requests

using Shared;

public class ParticipantDietsDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required List<AllergenDto> Allergens { get; set; }

}

