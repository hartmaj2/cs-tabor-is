using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;

[ApiController]
[Route("api/participants")]

public class ParticipantController : ControllerBase
{

    private readonly ParticipantsDbContext _context;

    public ParticipantController(ParticipantsDbContext context)
    {
        _context = context;
    }

    private static readonly string[] FirstNames = new[]
    {
        "James", "Olivia", "Liam", "Emma", "Noah", "Ava", "Lucas", "Sophia", "Mason", "Isabella"
    };

    private static readonly string[] LastNames = new[]
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez"
    };

    public static IList<Participant> participants;

    static ParticipantController()
    {
        participants = new List<Participant>();
        var random = new Random();
        for (int i = 0; i < 3; i++)
        {
            participants.Add(
                new Participant {
                    FirstName = FirstNames[random.Next(FirstNames.Length)], 
                    LastName = LastNames[random.Next(LastNames.Length)], 
                    Age = random.Next(5,18)
                    }
            );
        }
    }

    [HttpGet]
    public IEnumerable<Participant> GetParticipants()
    {
        return participants;
    }

    [HttpPost]
    public IActionResult CreateParticipant([FromBody] Participant participant)
    {
        participants.Add(participant);
        return CreatedAtAction(nameof(GetParticipants),participant);
    }
}