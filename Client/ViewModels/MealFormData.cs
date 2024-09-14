using System.ComponentModel.DataAnnotations;
using System.Data.Common;

public class MealFormData
{
    public int Id { get ; set; } 

    [Required(ErrorMessage = "Meal name is required.")]
    public string? Name { get; set;}

    
    [Required(ErrorMessage = "Meal type is required.")]
    public string? MealType { get; set; }

    public IList<AllergenSelection>? AllergenSelections;


    public MealDto ConvertToMealDto(MealTime mealTime, DateOnly date)
    {
        return new MealDto() 
            {
                Id = Id,
                Name = Name!,
                MealTime = mealTime,
                Type = Enum.Parse<MealType>(MealType!),
                Date = date,
                // Add a corresponding AllergenDto only when the selection IsSelected
                Allergens = AllergenSelections!.Where(selection => selection.IsSelected).Select(selection => new AllergenDto {Name = selection.Name}).ToList()
            };
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
    public static MealFormData ConvertToMealFormData(this MealDto mealDto, IEnumerable<AllergenDto> AllAllergens)
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

