using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// This model represents the associative table that tracks what participants have which diets
// Represents what participant has "allergy" to what (what this participant can't eat)

public class ParticipantAllergen
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Participant")]
    public int ParticipantId { get; set; }

    public Participant? Participant { get; set; } // Navigation property to which the foreign key above points

    [ForeignKey("Allergen")]
    public int AllergenId { get; set; }

    public Allergen? Allergen { get; set; } // navigation property that can be used to get the particular Allergen entry for this id
}