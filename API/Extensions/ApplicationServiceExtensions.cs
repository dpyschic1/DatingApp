using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //Create a database context service using the connection string in app configurations 
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }
            );

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>(); //Add a scoped token service to the container for JWT
            services.AddScoped<IUserRepository, UserRepository>(); // Add a scoped user repository service so it is injectable in our User controller
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Add automapper so it is injectable
            return services;
        }
    }
}