using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Meal
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MealTime MealTime { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MealType Type { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    public ICollection<MealAllergen>? MealAllergens { get; set; }

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