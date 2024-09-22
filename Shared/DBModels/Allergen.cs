using System.ComponentModel.DataAnnotations;

// Allergen Database Model

public class Allergen
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    // Navigation property used to retrieve all meals with this allergen if necessary
    public ICollection<MealAllergen>? MealAllergens { get; set; }
}