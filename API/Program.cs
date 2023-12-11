using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration); // using extension methods to tidy up the program.cs file

builder.Services.AddIdentityServices(builder.Configuration); // using extension methods 


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials() //allow SignalR to authenticate from client to server
.WithOrigins("https://localhost:4200")); //Allow cross origin access

app.UseAuthentication(); // Do you have a valid token?
app.UseAuthorization(); // what are you allowed to do?

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();  //create a scope against which to provide services
var services = scope.ServiceProvider; // create service container to use services
try
{
    var context = services.GetRequiredService<DataContext>(); // Save the data context in context
    var userManager = services.GetRequiredService<UserManager<AppUser>>(); // Get the user manager service to pass into seed user
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync(); // Migrate the database and create if not available
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]"); //Remove connections upon reset
    await Seed.SeedUsers(userManager, roleManager); //Seed the data
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
