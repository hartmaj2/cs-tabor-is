
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class MealCreateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealTime MealTime { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MealType Type { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public List<AllergenDto> Allergens { get; set; }
}

public class AllergenDto
{
    
    [Required]
    public string Name { get; set; }
}

public static class AllergenExtensions
{
    public static AllergenDto ToAllergenDto(this Allergen allergen)
    {
        return new AllergenDto { Name = allergen.Name};
    }
}
