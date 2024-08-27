using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/participants")]

public class ParticipantController : ControllerBase
{

    private readonly ParticipantsDbContext _context;

    public ParticipantController(ParticipantsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Participant> GetParticipantsFromDb()
    {
        return _context.Participants.ToList<Participant>();
    }

    [HttpPost]
    public IActionResult CreateParticipantDb([FromBody] Participant participant)
    {
        _context.Participants.Add(participant);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipantsFromDb),participant);
    }

}