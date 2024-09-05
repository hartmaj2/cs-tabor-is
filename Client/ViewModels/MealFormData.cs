using System.ComponentModel.DataAnnotations;

public class MealFormData
{
    [Required(ErrorMessage = "Meal name is required.")]
    public string? Name { get; set;}

    
    [Required(ErrorMessage = "Meal type is required.")]
    public string? MealType { get; set; }

    public IList<AllergenSelection>? AllergenSelections;

    // Used to bind IsSelected property to the EditForm checkboxes
    public class AllergenSelection
    {
        public required string Name { get; init; }
        public bool IsSelected { get; set; }
    }
}