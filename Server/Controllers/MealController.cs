using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;



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
            Allergen allergen = _context.Allergens.FirstOrDefault(a => a.Name == allergenDto.Name);
            meal.MealAllergens.Add(new MealAllergen {AllergenId = allergen.Id});
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
                    Allergens = _context.MealAllergens
                        .Where(ma => ma.MealId == meal.Id)
                        .Select(ma => ma.Allergen.ToAllergenDto())
                        .ToList<AllergenDto>(),
                }
        );
    }

}