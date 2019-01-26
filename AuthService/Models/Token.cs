using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class UserTokens
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}
