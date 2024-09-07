using System.ComponentModel.DataAnnotations;

namespace Shared;

// Participant model for the database

public class Participant
{
    [Key]
    public int Id {get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public int? Age { get; set; }
    [Required]
    public string? PhoneNumber {get; set; }
    [Required]
    public string? BirthNumber {get; set; }

    public ICollection<ParticipantAllergen>? ParticipantAllergens { get; set; }

}