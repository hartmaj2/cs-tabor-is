using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

// This model represents the association table between participants and allergens
// Represents what participant has "allergy" to what (what this participant can't eat)

public class ParticipantAllergen
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Participant")]
    public int ParticipantId { get; set; }

    public Participant? Participant { get; set; } // this is the navigation property to which the foreign key above points

    [ForeignKey("Allergen")]
    public int AllergenId { get; set; }

    public Allergen? Allergen { get; set; } // navigation property for AllergenId
}