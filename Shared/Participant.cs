using System.ComponentModel.DataAnnotations;

namespace Shared;

public class Participant
{
    [Key]
    public int Id {get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int Age { get; init; }
}