using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt => 
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<DataContext>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //Start an authentication service
                .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters // provide it with validation parameters
                        {
                            ValidateIssuerSigningKey = true, // check if the token issued is valid
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                    UTF8.GetBytes(config["TokenKey"])), // pass in the actual key
                            ValidateIssuer = false, // The issuer is the server
                            ValidateAudience = false // ditto for the audience
                        };
                        options.Events = new JwtBearerEvents // provide the bearer with events for SignalR
                        {
                            OnMessageReceived = context => 
                            {
                                var accessToken = context.Request.Query["access_token"]; //pass up the access token as a query
                            
                                var path = context.HttpContext.Request.Path; // pass the path of the current http context
                                if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) // check if the token is empty or the path starts with /hubs
                                {
                                    context.Token = accessToken; //gives SignalR access to bearer token
                                }

                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
            });

            return services;

        }
    }
}