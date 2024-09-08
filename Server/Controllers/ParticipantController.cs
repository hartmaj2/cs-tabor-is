using Microsoft.AspNetCore.Mvc;
using Shared;
using Server.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/participants")]

public class ParticipantController : ControllerBase
{

    private readonly TaborIsDbContext _context;

    // The context gets injected using dependency injection
    public ParticipantController(TaborIsDbContext context)
    {
        _context = context;
    }

    // Gets participant by id, it is important to eagerly load the participant Allergens before we convert him (the database does not automatically include allergens)
    [HttpGet("{id:int}")]
    public ParticipantDto? GetParticipantById(int id)
    {
        return _context.Participants
            .Include(participant => participant.ParticipantAllergens)!
            .ThenInclude(pa => pa.Allergen)
            .First(participant => participant.Id == id)
            .ConvertToParticipantDto();
    }

    // Gets the list of participants from the participant table
    // Also transfers them to dtos so allergens are sent as list of allergendtos
    [HttpGet("all")]
    public IEnumerable<ParticipantDto> GetParticipants()
    {
        return _context.Participants
            .Include(p => p.ParticipantAllergens)!
            .ThenInclude(pa => pa.Allergen)
            .ToList<Participant>()
            .Select(p => p.ConvertToParticipantDto());
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
    public IActionResult CreateParticipantDb([FromBody] ParticipantDto participantDto)
    {
        _context.Participants.Add(participantDto.ConvertToParticipant(_context));
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipants),participantDto);
    }

    // Adds a whole list of participants
    [HttpPost("add-many")]
    public IActionResult AddMultipleParticipants([FromBody] ICollection<ParticipantDto> participants)
    {
        _context.Participants.AddRange(participants.Select(participant => participant.ConvertToParticipant(_context)));
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetParticipants),participants);
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

// Extension is used because I need the DbContext so can't just put it in ParticipantDto class which doesn't have reference to this context
public static class ParticipantDtoExtensions
{
    public static Participant ConvertToParticipant(this ParticipantDto participantDto, TaborIsDbContext _context)
    {
        var participant = new Participant
        {
            Id = participantDto.Id,
            FirstName = participantDto.FirstName,
            LastName = participantDto.LastName,
            Age = participantDto.Age,
            PhoneNumber = participantDto.PhoneNumber,
            BirthNumber = participantDto.BirthNumber
        };

        participant.ParticipantAllergens = new List<ParticipantAllergen>();
        foreach (AllergenDto allergenDto in participantDto.Allergens)
        {
            Allergen? allergen = _context.Allergens.First(a => a.Name == allergenDto.Name);
            // Here it is necessary to set ParticipantId = participant.Id (when editing participant, I need it to create an entry in the ParticipantAllergens association table)
            participant.ParticipantAllergens.Add(new ParticipantAllergen {AllergenId = allergen!.Id, ParticipantId = participant.Id}); 
        }
        return participant;
    }
}

