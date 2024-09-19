using System.ComponentModel.DataAnnotations;

// Class used by the EditForm to validate, if required properties were set

public class MealFormData
{
    public int Id { get ; set; } // not set by user but having the fields allows for better conversion to MealDto

    [Required(ErrorMessage = "Meal name is required.")]
    public required string Name { get; set;}
    
    [Required(ErrorMessage = "Meal type is required.")]
    public required string MealType { get; set; }

    // In contract to MealDto, this class contains list of allergen selections 
    // Allergen selections are a combination of the name of the allergen and indicator whether it was selected
    public IList<AllergenSelection>? AllergenSelections;

    // Conversion method used after form submit when sending request to api 
    public MealDto ToMealDto(MealTime mealTime, DateOnly date)
    {
        return new MealDto() 
            {
                Id = Id,
                Name = Name,
                MealTime = mealTime,
                Type = Enum.Parse<MealType>(MealType),
                Date = date,
                // Add a corresponding AllergenDto only when the selection IsSelected
                Allergens = AllergenSelections!.Where(selection => selection.IsSelected).Select(selection => new AllergenDto {Name = selection.Name}).ToList()
            };
    }

    public static MealFormData CreateDefault()
    {
        return new MealFormData(){Name = string.Empty, MealType = string.Empty};
    }
    
}


// Used to bind IsSelected property to the EditForm checkboxes
public class AllergenSelection
{
    public required string Name { get; init; }
    public bool IsSelected { get; set; }
}

public static class MealDtoExtensions
{
    // Conversion method from MealDto to MealFormData, used when passing data to editMealModal in Menu
    public static MealFormData ToMealFormData(this MealDto mealDto, IEnumerable<AllergenDto> AllAllergens)
    {
        var mealFormData = new MealFormData()
            {
                Id = mealDto.Id,
                Name = mealDto.Name,
                MealType = mealDto.Type.ToString(),
                AllergenSelections = AllAllergens.Select(allergen => new AllergenSelection {Name = allergen.Name, IsSelected = false}).ToList()
            };
            
        // Mark all allergens of mealDto as true in AllergenSelections of the mealFormData
        foreach (var selection in mealFormData.AllergenSelections)
        {
            if (mealDto.Allergens.Select(allergen => allergen.Name).Contains(selection.Name))
            {
                selection.IsSelected = true;
            }
        }
        return mealFormData;
    }
}

