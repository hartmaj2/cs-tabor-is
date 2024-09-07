using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/participants")]

public class ParticipantController : ControllerBase
{

    private readonly ParticipantsDbContext _context;

    // The context gets injected using dependency injection
    public ParticipantController(ParticipantsDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public Participant? GetParticipantById(int id)
    {
        return _context.Participants.Find(id);
    }

    [HttpGet("all-diets")]
    public IEnumerable<ParticipantDto> GetParticipants()
    {
        return _context.Participants
            .Include(p => p.ParticipantAllergens)!
            .ThenInclude(pa => pa.Allergen)
            .ToList<Participant>()
            .Select(p => p.ConvertToParticipantDietsDto(_context));
    }

    // Gets the list of participants from the participant table
    [HttpGet("all")]
    public IEnumerable<Participant> GetParticipantsFromDb()
    {
        return _context.Participants.ToList<Participant>();
    }

    [HttpPost("edit/{id:int}")]
    public IActionResult EditParticipant(int id, [FromBody] Participant updatedParticipant)
    {
        Participant oldParticipant = _context.Participants.Find(id)!;
        _context.Entry(oldParticipant).CurrentValues.SetValues(updatedParticipant);
        _context.SaveChanges();
        return NoContent();
    }

    // Adds a participant to the participant table
    [HttpPost("add")]
    public IActionResult CreateParticipantDb([FromBody] Participant participant)
    {
        _context.Participants.Add(participant);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipantsFromDb),participant);
    }

    // Adds a whole list of participants
    [HttpPost("add-many")]
    public IActionResult AddMultipleAllergens([FromBody] ICollection<Participant> participants)
    {
        _context.Participants.AddRange(participants);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipantsFromDb),participants);
    }

    // Deletes single participant with given id
    [HttpDelete("delete/{id:int}")]
    public IActionResult DeleteParticipant(int id)
    {
        Participant toDelete = _context.Participants.Find(id)!;
        _context.Participants.Remove(toDelete);
        _context.SaveChanges();
        return NoContent();
    }

    // Deletes everything from the participant table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllParticipantsDb()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM [Participants]");
        return NoContent();
    }

}

public static class ParticipantExtensions
{
    // Converts a Participant to Participant diets dto, the navigation properties ParticipantAllergens and Allergen must be loaded from db explicitly using Include
    public static ParticipantDto ConvertToParticipantDietsDto(this Participant thisParticipant,ParticipantsDbContext _context)
    {
        return new ParticipantDto()
        {
            Id = thisParticipant.Id,
            FirstName = thisParticipant.FirstName,
            LastName = thisParticipant.LastName,
            Age = thisParticipant.Age,
            PhoneNumber = thisParticipant.PhoneNumber,
            BirthNumber = thisParticipant.BirthNumber,
            Allergens = thisParticipant.ParticipantAllergens!.Select(pa => pa.Allergen!.ToAllergenDto()).ToList()
        };
    }
}