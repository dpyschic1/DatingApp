using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {

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

                    });
                    return services;

        }
    }
}