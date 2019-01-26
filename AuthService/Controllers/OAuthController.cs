using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Data;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly UsersDb _usersDb;
        private readonly AppsDb _appsDb;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public OAuthController(UsersDb usersDb, AppsDb appsDb,
            IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _usersDb = usersDb;
            _appsDb = appsDb;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            if (!_usersDb.Users.Any())
            {
                _usersDb.Users.Add(new User
                {
                    Username = "First",
                    Password = _passwordHasher.GenerateIdentityV3Hash("Password")
                });
                _usersDb.Users.Add(new User
                {
                    Username = "Second",
                    Password = _passwordHasher.GenerateIdentityV3Hash("Password")
                });

                _usersDb.SaveChanges();
            }
            if (!_appsDb.Users.Any())
            {
                _appsDb.Users.Add(new AppUser
                {
                    client_id = "clientid",
                    appSecret = _passwordHasher.GenerateIdentityV3Hash("secret")
                });
                _appsDb.SaveChanges();
            }
        }

        [HttpPost]//
        [Route("token")] // /oauth/token
        public async Task<ActionResult<UserTokens>> GetTokens([FromBody] AppUser _user)
        {
            var user = _appsDb.Users.SingleOrDefault(u => u.client_id == _user.client_id);
            //знакомое приложение
            if (user == null)
                return BadRequest("Unknown App");
            if (!_passwordHasher.VerifyIdentityV3Hash(_user.appSecret, user.appSecret)
                || user.code != _user.code)
                return BadRequest();
            
            var usersClaims = new[]
            {
                new Claim(ClaimTypes.Name, _user.client_id)
            };

            var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
            var refreshToken = _tokenService.GenerateRefreshToken(usersClaims);

            user.refreshToken = refreshToken;
            await _appsDb.SaveChangesAsync();

            return new UserTokens
            {
                token = jwtToken,
                refreshToken = refreshToken
            };
        }

        //app: client_id, secret, authcode
        [HttpPost]//
        [Route("authcode")] // /oauth/authcode
        public async Task<ActionResult<RedUrl>> GetAuthCode([FromBody] User _user,[FromQuery]string client_id = "clientid", [FromQuery]string redirect_uri = "http://localhost:8701/return", [FromQuery]string response_type = "code")
        {
            string username = _user.Username;
            string password = _user.Password;
            var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            if (user == null || !_passwordHasher.VerifyIdentityV3Hash(password, user.Password))
                return BadRequest("Can't authorize user");
            //юзер подтвержден

            var app = _appsDb.Users.SingleOrDefault(u => u.client_id == client_id);
            //знакомое приложение
            if (app == null)
                return BadRequest("Unknown App");

            app.code = GetAuthCode();
            await _appsDb.SaveChangesAsync();

            string url = redirect_uri + "?code=" + app.code;
            return new RedUrl()
            {
                red = url
            };

        }

        [HttpGet]//
        [Route("authorize")] // /oauth/authorize
        public async Task<ActionResult<string>> LoginUser([FromQuery]string client_id = "clientid", [FromQuery]string redirect_uri = "http://localhost:8701/return", [FromQuery]string response_type = "code")
        {
            var user = _appsDb.Users.SingleOrDefault(u => u.client_id == client_id);
            if (user == null)
                return BadRequest("Unknown App");
            string url = "http://localhost:8701/oauth2/login" +"?redirect_uri=" + redirect_uri + "&client_id=" + client_id;
            return Redirect(url);
        }

        [HttpGet]//
        [Authorize]
        [Route("validate")]
        public async Task<IActionResult> Validate()
        {
            string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal == null) 
                return BadRequest();
            else
                return Ok();
        }

        [HttpPost]//
        [Route("refreshtokens")]
        public async Task<ActionResult<UserTokens>> Refresh([FromBody] UserTokens tokens)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.token);
            if (principal == null)
                return Unauthorized();
            var username = principal.Identity.Name;
            string newJwtToken;
            string newRefreshToken;


            var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            if (user != null && user.RefreshToken == tokens.refreshToken)
            {
                newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                newRefreshToken = _tokenService.GenerateRefreshToken(principal.Claims);

                user.RefreshToken = newRefreshToken;
                await _usersDb.SaveChangesAsync();
            }
            else
            {
                var app = _appsDb.Users.SingleOrDefault(u => u.client_id == username);
                if (app != null && app.refreshToken == tokens.refreshToken)
                {
                    newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                    newRefreshToken = _tokenService.GenerateRefreshToken(principal.Claims);

                    app.refreshToken = newRefreshToken;
                    await _appsDb.SaveChangesAsync();
                }
                else 
                    return Unauthorized();
                
            }

            return new UserTokens
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            };
        }
  
        private string GetAuthCode()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next().ToString();
        }

    }
}