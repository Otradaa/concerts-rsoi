using System.Collections.Generic;
using System.Security.Claims;

namespace AuthService.Services
{
    public interface ITokenService
    {
         string GenerateAccessToken(IEnumerable<Claim> claims);         
         string GenerateRefreshToken(IEnumerable<Claim> claims);    
         ClaimsPrincipal GetPrincipalFromExpiredToken(string token);            
    }
}