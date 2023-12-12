using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {


            services.AddCors();
            services.AddScoped<ITokenService, TokenService>(); //Add a scoped token service to the container for JWT
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Add automapper so it is injectable
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); // get the cloudinary configurations
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>(); //Add scoped service of unit of work which has user, likes and message repositories
            return services;
        }
    }
}