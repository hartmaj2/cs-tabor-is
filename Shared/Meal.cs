using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Meal
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; } // Name of the meal

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MealTime MealTime { get; set; } // This is either Lunch or Dinner

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MealType Type { get; set; } // Either Soup or Main

    [Required]
    public required DateOnly Date { get; set; } // The date when the food will be served

    public ICollection<MealAllergen>? MealAllergens { get; set; } // Represents allergens that are present in the food

}

public enum MealTime
{
    Lunch,
    Dinner
}

public enum MealType
{
    Soup,
    Main
}