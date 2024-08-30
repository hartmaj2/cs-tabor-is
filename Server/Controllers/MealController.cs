using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/menu")]

public class MenuController : ControllerBase
{

    private readonly ParticipantsDbContext _context;

    // The context gets injected using dependency injection
    public MenuController(ParticipantsDbContext context)
    {
        _context = context;
    }

    // Gets the list of all meals from the meals table
    [HttpGet("allergens/all")]
    public IEnumerable<Allergen> GetAllAllergens()
    {
        return _context.Allergens.ToList<Allergen>();
    }

    // Gets the list of all meals from the meals table
    [HttpPost("allergens/add")]
    public IActionResult AddNewAllergen([FromBody] Allergen allergen)
    {
        _context.Allergens.Add(allergen);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllAllergens),allergen);
    }

    // Adds a whole list of allergens
    [HttpPost("allergens/add-many")]
    public IActionResult AddMultipleAllergens([FromBody] ICollection<Allergen> allergens)
    {
        _context.Allergens.AddRange(allergens);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllAllergens),allergens);
    }

    // Deletes single allergen with given id
    [HttpDelete("allergens/delete/{id:int}")]
    public IActionResult DeleteAllergen(int id)
    {
        Allergen toDelete = _context.Allergens.Find(id)!;
        _context.Allergens.Remove(toDelete);
        _context.SaveChanges();
        return NoContent();
    }





}