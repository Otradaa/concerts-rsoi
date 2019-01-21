using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<UsersToken> Login(User user);
        Task<UsersToken> RefreshTokens(UsersToken token);
        Task<bool> ValidateToken(string accessToken);
    }
}
