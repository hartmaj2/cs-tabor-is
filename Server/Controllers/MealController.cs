using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;

[ApiController]
[Route("api/meals")]

public class MealsController : ControllerBase
{

    private readonly ParticipantsDbContext _context;

    // The context gets injected using dependency injection
    public MealsController(ParticipantsDbContext context)
    {
        _context = context;
    }

    // Adds a meal to the meals table
    [HttpPost("add")]
    public IActionResult AddMeal([FromBody] MealDto received)
    {
        AddSingleMeal(received);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllMeals),received);
    }

    // Adds a list of meals to the meals table
    [HttpPost("add-many")]
    public IActionResult AddMeals([FromBody] ICollection<MealDto> receiveds)
    {
        foreach (var mealDto in receiveds)
        {
            AddSingleMeal(mealDto);
        }
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllMeals),receiveds);
    }

    // Gets the list of all meals from the meals table
    // I have to map all Meal objects to MealCreateDtos because I want to be getting a list of all allergens for every meal
    // (also uses eager loading with Include and then Include)
    [HttpGet("all")]
    public IEnumerable<MealDto> GetAllMeals()
    {
        return _context.Meals
            .Include( meal => meal.MealAllergens!) // this and the following line are necessary, it eageryly loads the collection of MealAllergens for all meals so it is ready to be used by ToMealDto function
            .ThenInclude( mealAllergen => mealAllergen.Allergen)
            .Select( meal => meal.ToMealDto());
        
    }

    // Uses eager loading for navigation entity MealAllergens (navigation entity represents a relationship to another entity or collection of entities)
    // The code would not work without it when using ToMealDto function (the mealAllergens collection would appear empty)
    [HttpGet("{date}")]
    public IEnumerable<MealDto> GetMealsByDate(DateOnly date)
    {
        return _context.Meals
        .Where( meal => meal.Date == date) 
        .Include( meal => meal.MealAllergens!) // this and the following line are necessary, it eageryly loads the collection of MealAllergens for all meals so it is ready to be used by ToMealDto function
        .ThenInclude( mealAllergen => mealAllergen.Allergen)
        .Select( meal => meal.ToMealDto());

    }

    // Gets the list of names of possible enum values for meal type
    [HttpGet("meal-types")]
    public IEnumerable<string> GetMealTypes()
    {
        
        return Enum.GetNames<MealType>();
    }

    [HttpPost("edit/{id:int}")]
    public IActionResult EditMeal(int id, [FromBody] MealDto updatedMeal)
    {
        Meal oldMeal = _context.Meals.Where(meal => meal.Id == id).Include(meal => meal.MealAllergens).First();
        Meal newMeal = updatedMeal.ConvertToMeal(_context);
        _context.Entry(oldMeal).CurrentValues.SetValues(newMeal);

        // I also need to remove old entries in the association tables (this is why i needed to call .Include for old meal)
        foreach (var mealAllergen in oldMeal.MealAllergens!)
        {
            _context.MealAllergens.Remove(mealAllergen);
        }
        // Add new entries to the association table
        foreach (var mealAllergen in newMeal.MealAllergens!)
        {
            _context.MealAllergens.Add(mealAllergen);
        }
        _context.SaveChanges();
        return NoContent();
    }

    // Deletes single meal based on id
    [HttpDelete("{id:int}")]
    public IActionResult DeleteMeal(int id)
    {
        Meal mealToDelete = _context.Meals.Find(id)!;
        _context.Remove(mealToDelete);
        _context.SaveChanges();
        return NoContent();
    }

    // Deletes everything from the meals table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllMeals()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM [Meals]");
        return NoContent();
    }

    // A function to add a single meal
    // Is called repeatedly when adding a list of meals
    private void AddSingleMeal(MealDto mealDto)
    {
        _context.Meals.Add(mealDto.ConvertToMeal(_context));
    }

}

public static class MealDtoExtensions
{
    public static Meal ConvertToMeal(this MealDto mealDto, ParticipantsDbContext _context)
    {
        var meal = new Meal
        {
            Id = mealDto.Id,
            Name = mealDto.Name,
            MealTime = mealDto.MealTime,
            Type = mealDto.Type,
            Date = mealDto.Date,
        };

        meal.MealAllergens = new List<MealAllergen>();
        foreach (AllergenDto allergenDto in mealDto.Allergens)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(a => a.Name == allergenDto.Name);
            // Here it is necessary to set MealId = meal.Id (when editing meal, I need it to create an entry in the MealAllergens association table)
            meal.MealAllergens.Add(new MealAllergen {AllergenId = allergen!.Id, MealId = meal.Id}); 
        }
        return meal;
    }
}