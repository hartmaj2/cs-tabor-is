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
    public IEnumerable<Order> GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    // Gets the list of all meals from the meals table
    [HttpPost("add")]
    public IActionResult AddNewOrder([FromBody] Order Order)
    {
        _context.Orders.Add(Order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllOrders),Order);
    }

    // Adds a whole list of Orders
    [HttpPost("add-many")]
    public IActionResult AddMultipleOrders([FromBody] ICollection<Order> Orders)
    {
        _context.Orders.AddRange(Orders);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllOrders),Orders);
    }

    // Deletes everything from the Orders table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllOrders()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM [Orders]");
        return NoContent();
    }

}