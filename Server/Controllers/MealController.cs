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

    // Gets all meals from the Meals table
    [HttpPost("add")]
    public IActionResult AddMeal([FromBody] MealDto received)
    {


        var meal = new Meal
        {
            Name = received.Name,
            MealTime = received.MealTime,
            Type = received.Type,
            Date = received.Date,
            // MealAllergens = received.Allergens.Select(a => new MealAllergen
            // {
            //     AllergenId = a.Id
            // }).ToList()
        };

        meal.MealAllergens = new List<MealAllergen>();
        foreach (AllergenDto allergenDto in received.Allergens)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(a => a.Name == allergenDto.Name);
            meal.MealAllergens.Add(new MealAllergen {AllergenId = allergen!.Id});
        }

        _context.Meals.Add(meal);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllMeals),received);
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

}