using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); //Get the token key from configurations
        }
        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), //Creat jwt registers claims
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)  //Add the username as the unique name to the claim
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //Add the role to the claims

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // get the credentials from the key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //save the subject
                Expires = DateTime.Now.AddDays(7),// add expiry of 7 days
                SigningCredentials = creds // And save the signing credentials from creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // create a jwt token handler to use commands on our token
            var token = tokenHandler.CreateToken(tokenDescriptor); // create the token

            return tokenHandler.WriteToken(token);
        }
    }
}