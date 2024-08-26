var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // This adds all classes marked with [ApiController] as a service

var app = builder.Build();

app.UseBlazorFrameworkFiles(); // This is necessary so the server knows it should use the Blazor files in Client
// app.UseStaticFiles();

app.MapControllers(); // This adds the controller services to our routes

/* 
*   The following line of code is necessary to be able to give control to the Blazor Web Assembly so I can address Client side pages
*/
app.MapFallbackToFile("index.html");

app.Run();
