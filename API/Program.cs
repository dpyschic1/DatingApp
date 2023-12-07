using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
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
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); //Allow cross origin access

app.UseAuthentication(); // Do you have a valid token?
app.UseAuthorization(); // what are you allowed to do?

app.MapControllers();

using var scope = app.Services.CreateScope();  //create a scope against which to provide services
var services = scope.ServiceProvider; // create service container to use services
try
{
    var context = services.GetRequiredService<DataContext>(); // Save the data context in context
    var userManager = services.GetRequiredService<UserManager<AppUser>>(); // Get the user manager service to pass into seed user
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync(); // Migrate the database and create if not available
    await Seed.SeedUsers(userManager, roleManager); //Seed the data
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
