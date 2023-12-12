using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        
        //Constructor for exception handling middleware, taking three paramters a request delegate to handle the request
        //A logger to log and display to the console
        //And an environment object to check if we are in dev or not
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;  
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); //Send the exception to terminal
                context.Response.ContentType = "application/json"; //return a response to the client of type json
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //cast to int and send 500 error

                var response =  _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) // send stack trace if in dev mode
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error"); //otherwise send Internal server error

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json); // send the response 
            }
        }
        
    }
}