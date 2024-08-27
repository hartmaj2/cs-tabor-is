using System.ComponentModel.DataAnnotations;

namespace Shared;

public class Participant
{
    [Key]
    public int Id {get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int Age { get; set; }
    public required string PhoneNumber {get; set; }
    public required string BirthNumber {get; set; }
}