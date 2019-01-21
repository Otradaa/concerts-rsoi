using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class Token: IToken
    {
        private readonly AuthOptions options;


        public Token(AuthOptions options)
        {
            this.options = options;
        }

        public string GenerateAccessToken(string id, string userName)
        {
            return this.GenerateJwtToken(id, userName, options.AccessTokenLifetime);
        }

        public string GenerateRefreshToken(string id, string userName)
        {
            return this.GenerateJwtToken(id, userName, options.RefreshTokenLifetime);
        }
        private string GenerateJwtToken(string id, string userName, double lifeTime)
        {
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, id),
             //   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              //  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Iss, options.ISSUER),
                //new Claim(JwtRegisteredClaimNames.Iss, options.AUDIENCE),
            };


            var key = options.GetSymmetricSecurityKey();

            var credits = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(options.ISSUER, options.AUDIENCE, claims, expires: DateTime.Now.Add(TimeSpan.FromMinutes(lifeTime)),
                signingCredentials: credits);


            return new JwtSecurityTokenHandler().WriteToken(token); //token;
        }



        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = options.GetParameters();

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

        public string GetUsernameFromToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        private ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                var validationParameters = options.GetParameters();
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      validationParameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
