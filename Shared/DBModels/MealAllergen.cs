using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Represents entry in associative table that tracks the relation between what meals contain which allergens

public class MealAllergen
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Meal")]
    public int MealId { get; set; }
    public Meal? Meal { get; set; }

    [ForeignKey("Allergen")]
    public int AllergenId { get; set; }
    public Allergen? Allergen { get; set; }
}