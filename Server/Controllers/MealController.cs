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
    [HttpGet("all")]
    public IEnumerable<MealDto> GetAllMeals()
    {
        return _context.Meals
            .Select( meal => new MealDto
                {
                    Name = meal.Name,
                    MealTime = meal.MealTime,
                    Type = meal.Type,
                    Date = meal.Date,
                    Allergens = meal.MealAllergens!.Select(allergen => allergen.Allergen!.ToAllergenDto()).ToList()
                }
        );
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
        var meal = new Meal
        {
            Name = mealDto.Name,
            MealTime = mealDto.MealTime,
            Type = mealDto.Type,
            Date = mealDto.Date,
        };

        meal.MealAllergens = new List<MealAllergen>();
        foreach (AllergenDto allergenDto in mealDto.Allergens)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(a => a.Name == allergenDto.Name);
            meal.MealAllergens.Add(new MealAllergen {AllergenId = allergen!.Id});
        }

        _context.Meals.Add(meal);
    }

}