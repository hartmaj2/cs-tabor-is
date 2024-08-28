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

    // Gets the list of participants from the participant table
    [HttpGet("all")]
    public IEnumerable<Participant> GetParticipantsFromDb()
    {
        return _context.Participants.ToList<Participant>();
    }

    // Adds a participant to the participant table
    [HttpPost]
    public IActionResult CreateParticipantDb([FromBody] Participant participant)
    {
        _context.Participants.Add(participant);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipantsFromDb),participant);
    }


    // Deletes everything from the participant table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllParticipantsDb()
    {
        _context.Database.ExecuteSqlRaw("TRUNCATE TABLE [Participants]");
        return NoContent();
    }

}