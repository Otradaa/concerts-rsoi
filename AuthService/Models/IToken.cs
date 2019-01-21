using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public interface IToken
    {
        string GenerateRefreshToken(string id, string userName);

        string GenerateAccessToken(string id, string userName);
        bool ValidateToken(string token);
        string GetUsernameFromToken(string token);
    }
}
