using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using static AuthService.Models.Account;
using AuthService.Data;
using AuthService.Services;
using User = AuthService.Data.User;
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
     //   private AppUser appUser;
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
            //if (appUser == null)
            //    appUser = new AppUser() { client_id = "clientid", appSecret = "secret", code = "" };
        }

/*
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] UserTokens tokens)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.token);
            if (principal == null)
                return Unauthorized();
            var client_id = principal.Identity.Name; //this is mapped to the Name claim by default

            //var app = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            if (client_id != appUser.client_id || appUser.refreshToken != tokens.refreshToken)
                return BadRequest();

            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken(principal.Claims);

            appUser.refreshToken = newRefreshToken;

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
*/



        [HttpPost]//
        [Route("token")] // /oauth/token
        public async Task<ActionResult<UserTokens>> GetTokens([FromBody] AppUser _user)
        {
            //if (user == null || !_passwordHasher.VerifyIdentityV3Hash(password, user.Password)) return BadRequest();
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
              //  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
        public async Task<ActionResult<RedUrl>> GetAuthCode([FromBody] Data.User _user,[FromQuery]string client_id = "clientid", [FromQuery]string redirect_uri = "http://localhost:8701/return", [FromQuery]string response_type = "code")
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
            //this.Response.ContentType = "text/plain";
            return new RedUrl()
            {
                red = url
            };

        }


            //редирект на форму с клиент айди

            //запрос на код
            //редирект на форму для входа клиента с клиент айди
            //на той форме кнопка подтвердить права которая 
        [HttpGet]//
        [Route("authorize")] // /oauth/authorize
        public async Task<ActionResult<string>> LoginUser([FromQuery]string client_id = "clientid", [FromQuery]string redirect_uri = "http://localhost:8701/return", [FromQuery]string response_type = "code")
        {
            var user = _appsDb.Users.SingleOrDefault(u => u.client_id == client_id);
            //знакомое приложение
            if (user == null)
                return BadRequest("Unknown App");
            string url = "http://localhost:8701/oauth2/login" +"?redirect_uri=" + redirect_uri + "&client_id=" + client_id;
            return Redirect(url);

            //редирект на форму с клиент айди
            // и обратно с формы запрос с токеном от юзера и клиент айди
            //если токен валиден то даем код приложению

            
        }

        [HttpGet]//
        [Authorize]
        [Route("validate")]
        public async Task<IActionResult> Validate()
        {
            string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);//???????????????????
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
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            string newJwtToken;
            string newRefreshToken;
            if (username != "clientid")////////////////////////////////////////////////////
            {
                var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
                if (user == null || user.RefreshToken != tokens.refreshToken) return Unauthorized();
                newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                newRefreshToken = _tokenService.GenerateRefreshToken(principal.Claims);

                user.RefreshToken = newRefreshToken;
                await _usersDb.SaveChangesAsync();
            }
            else
            {
                var user = _appsDb.Users.SingleOrDefault(u => u.client_id == username);
                if (user == null || user.refreshToken != tokens.refreshToken) 
                    return Unauthorized();
                newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                newRefreshToken = _tokenService.GenerateRefreshToken(principal.Claims);

                user.refreshToken = newRefreshToken;
                await _appsDb.SaveChangesAsync();

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