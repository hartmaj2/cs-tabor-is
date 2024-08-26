using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Shared;

[ApiController]
[Route("participants")]

public class ParticipantController : ControllerBase
{

    private static readonly string[] FirstNames = new[]
    {
        "James", "Olivia", "Liam", "Emma", "Noah", "Ava", "Lucas", "Sophia", "Mason", "Isabella"
    };

    private static readonly string[] LastNames = new[]
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez"
    };

    [HttpGet]
    public IEnumerable<Participant> Get()
    {
        var participants = new List<Participant>();
        var random = new Random();
        for (int i = 0; i < 10; i++)
        {
            participants.Add(
                new Participant(
                    FirstNames[random.Next(FirstNames.Length)],
                    LastNames[random.Next(LastNames.Length)],
                    random.Next(5,18)
                )
            );
        }
        return participants;
    }
}