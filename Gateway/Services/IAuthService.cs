using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IAuthService
    {
        Task<UserTokens> Login(User user);
        Task<UserTokens> RefreshTokens(UserTokens token);
        Task<bool> ValidateToken(string accessToken);
        Task<string> LoginApp();
        Task<UserTokens> GetTokens(AppUser user);
        Task<RedUrl> GetAuthCode(User user);

    }
}
