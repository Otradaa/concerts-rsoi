using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertsService.Auth
{
    public class TokenParameters
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }

        const string KEY = "AutorizationServiceSecretKey";
        public double TokenLifetime { get; } = 30;

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public TokenValidationParameters GetParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = ISSUER,
                ValidateAudience = true,
                ValidAudience = AUDIENCE,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }

    }
}
