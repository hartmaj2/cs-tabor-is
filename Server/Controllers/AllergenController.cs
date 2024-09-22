using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/allergens")]

// Used to handle requests related to allergens/diets that the meals/participants can contain/have
public class AllergensController : ControllerBase
{

    private readonly TaborIsDbContext _context;

    // The context gets injected automatically using dependency injection
    public AllergensController(TaborIsDbContext context)
    {
        _context = context;
    }

    // Gets the list of all allergens from the Allergens table
    [HttpGet("all")]
    public IEnumerable<AllergenDto> GetAllAllergens()
    {
        return _context.Allergens.ToList().Select(allergen => allergen.ToAllergenDto());
    }

    // Adds an allergen to the Allergens table
    [HttpPost("add")]
    public IActionResult AddNewAllergen([FromBody] AllergenDto allergen)
    {
        _context.Allergens.Add(allergen.ConvertToAllergen());
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllAllergens),allergen);
    }

    // Adds a whole list of allergens to the Allergens table (useful when populating empty table)
    [HttpPost("add-many")]
    public IActionResult AddMultipleAllergens([FromBody] ICollection<AllergenDto> allergens)
    {
        _context.Allergens.AddRange(allergens.Select(allergenDto => allergenDto.ConvertToAllergen()));
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllAllergens),allergens);
    }

    // Deletes single allergen with given id from the Allergens table
    [HttpDelete("delete/{id:int}")]
    public IActionResult DeleteAllergen(int id)
    {
        Allergen toDelete = _context.Allergens.Find(id)!;
        _context.Allergens.Remove(toDelete);
        _context.SaveChanges();
        return NoContent();
    }

    // Deletes everything from the Allergens table 
    // (this is better than truncating because it takes foreign keys into account and uses cascading to remove the corresponding values in associative tables)
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllAllergens()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM [Allergens]");
        return NoContent();
    }

}