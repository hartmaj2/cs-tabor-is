using Microsoft.EntityFrameworkCore;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // This adds all classes marked with [ApiController] as a service

// Adds the DbContext service to my builder, the connection string can be configured in appsettings.json
// Using dependency injection so my controller gets the DbContext in constructor automatically
builder.Services.AddDbContext<TaborIsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseBlazorFrameworkFiles(); // This is necessary so the server knows it should use the Blazor files in Client
app.UseStaticFiles(); // This is also necessary so Client Blazor can use the wwwroot static files

app.MapControllers(); // This adds the controller services to our routes

/* 
*   The following line of code is necessary to be able to give control to the Blazor Web Assembly so I can address Client side pages
*/
app.MapFallbackToFile("index.html");

app.Run();
