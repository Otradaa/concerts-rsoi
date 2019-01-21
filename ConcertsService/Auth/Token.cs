using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConcertsService.Auth
{
    public class Token : IToken
    {
        private readonly TokenParameters parameters;

        public Token(TokenParameters _parameters)
        {
            parameters = _parameters;
        }

        public string GenerateToken(string userName)//////////////////////////////////////////////////////////////
        {
            var claims = new[]{
             //   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              //  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Iss, parameters.ISSUER),
                //new Claim(JwtRegisteredClaimNames.Iss, options.AUDIENCE),
            };


            var key = parameters.GetSymmetricSecurityKey();

            var credits = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(parameters.ISSUER, parameters.AUDIENCE, claims, 
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(parameters.TokenLifetime)),
                signingCredentials: credits);


            return new JwtSecurityTokenHandler().WriteToken(token); //token;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = parameters.GetParameters();

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
