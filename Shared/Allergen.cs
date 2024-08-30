using System.ComponentModel.DataAnnotations;

public class Allergen
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<MealAllergen>? MealAllergens { get; set; }
}