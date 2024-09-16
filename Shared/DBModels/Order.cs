using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Represents an order that the participant can make for a certain meal

public class Order
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Participant")]
    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }
    
    [ForeignKey("Meal")]
    public int MealId { get; set; }
    public Meal? Meal { get; set; }

}