using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// These classes serve as the JSON templates to be communicated from client to server and vice versa
// I wanted to be able to send the meal as the list of its properties + a list of names of allergens

public class MealDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealTime MealTime { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealType Type { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    public required List<AllergenDto> Allergens { get; set; }
}

public class AllergenDto
{

    [Required]
    public required string Name { get; set; }
}

public static class AllergenExtensions
{
    public static AllergenDto ToAllergenDto(this Allergen allergen)
    {
        return new AllergenDto { Name = allergen.Name};
    }
}
