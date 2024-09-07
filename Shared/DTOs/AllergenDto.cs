using System.ComponentModel.DataAnnotations;

// Serve as the JSON templates to be communicated from client to server and vice versa
// I wanted to be able to send the meal as the list of its properties + a list of names of allergens

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