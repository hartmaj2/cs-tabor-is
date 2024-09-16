using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/orders")]

public class OrdersController : ControllerBase
{

    private readonly TaborIsDbContext _context;

    // The context gets injected using dependency injection
    public OrdersController(TaborIsDbContext context)
    {
        _context = context;
    }

    // Gets the list of all meals from the meals table
    [HttpGet("all")]
    public IEnumerable<OrderDto> GetAllOrders()
    {
        return _context.Orders.Select(order => order.ConvertToOrderDto());
    }

    // Gets the list of all meals from the meals table
    [HttpGet("meal/{id:int}")]
    public IEnumerable<OrderDto> GetMealOrders(int id)
    {
        var meal = _context.Meals.Include(meal => meal.Orders).First(meal => meal.Id == id);
        return meal.Orders!.Select(order => order.ConvertToOrderDto());
    }

    // Gets the list of all meals from the meals table
    [HttpPost("add")]
    public IActionResult AddNewOrder([FromBody] OrderDto orderDto)
    {
        _context.Orders.Add(orderDto.ConvertToOrder());
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllOrders),orderDto);
    }

    // Adds a whole list of Orders
    [HttpPost("add-many")]
    public IActionResult AddMultipleOrders([FromBody] ICollection<OrderDto> orders)
    {
        _context.Orders.AddRange(orders.Select(orderDto => orderDto.ConvertToOrder()));
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllOrders),orders);
    }

    // Deletes everything from the Orders table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllOrders()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM [Orders]");
        return NoContent();
    }

}