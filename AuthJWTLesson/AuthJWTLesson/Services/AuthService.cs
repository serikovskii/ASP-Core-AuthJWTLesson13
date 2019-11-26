using AuthJWTLesson.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthJWTLesson.Services
{
    public class AuthService
    {
        private readonly DataAccess.AppContext appContext;
        private readonly string jwtSecret;

        public AuthService(DataAccess.AppContext appContext, IOptions<SecretOptions> secretOptions)
        {
            this.appContext = appContext;
            jwtSecret = secretOptions.Value.JWTSecret;
        }
        // https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
        public async Task<string> Authenticate(string login, string password)
        {
            var existingUser = await appContext.Users.FirstOrDefaultAsync(user => user.Password == password && user.Username == login);
            if (existingUser == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
