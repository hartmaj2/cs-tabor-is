// OrderDto is used because:
//  1. I want to be consistent and 
//  2. I don't want to be sending null fields for Meal and Participant navigation properties (just send the ids instead)

public class OrderDto
{
    public int Id { get; set; }

    public required int ParticipantId { get; set; }

    public required int MealId { get; set; }

    public Order ConvertToOrder()
    {
        return new Order { Id = Id, ParticipantId = ParticipantId, MealId = MealId };
    }

}

public static class OrderExtensions
{
    public static OrderDto ConvertToOrderDto(this Order order)
    {
        return new OrderDto {Id = order.Id, ParticipantId = order.ParticipantId, MealId = order.MealId };
    }
}