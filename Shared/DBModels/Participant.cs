using System.ComponentModel.DataAnnotations;

// Participant model for the database

public class Participant
{
    [Key]
    public int Id {get; set; }
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public required string PhoneNumber {get; set; }
    [Required]
    public required string BirthNumber {get; set; }

    public ICollection<ParticipantAllergen>? ParticipantAllergens { get; set; } // Represents diets that this participant has

    public ICollection<Order>? Orders { get; set; } // Represents orders that this participant placed

}