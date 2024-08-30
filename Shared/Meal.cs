using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Meal
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealTime MealTime { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealType Type { get; set; }

    [Required]
    public DateTime Date { get; set; }

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