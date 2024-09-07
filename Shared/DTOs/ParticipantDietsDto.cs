public class ParticipantDietsDto
{

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required ICollection<AllergenDto> Allergens { get; set; }

}