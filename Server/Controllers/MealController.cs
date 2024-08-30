using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;


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
    public IActionResult AddMeal([FromBody] Meal meal)
    {
        _context.Meals.Add(meal);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllMeals),meal);
    }

    // Gets the list of all meals from the meals table
    [HttpGet("all")]
    public IEnumerable<Meal> GetAllMeals()
    {
        return _context.Meals.ToList<Meal>();
    }
}