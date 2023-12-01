using API.Extensions;
using API.Middleware;

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

app.Run();
