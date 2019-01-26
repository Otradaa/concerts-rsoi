using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AuthService.Data;
using AuthService.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserTokens>> Login([FromBody] User user)
        {
            var logged = await _authService.Login(user);
            if (logged == null)
                return BadRequest();
            return Ok(logged);
        }

        [HttpPost]
        [Route("authcode")]
        public async Task<ActionResult> GetAuthCode([FromBody] User user)
        {
            var result = await _authService.GetAuthCode(user);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Ok(result.Content.ReadAsAsync<RedUrl>().Result);
            }
            return BadRequest(result.ReasonPhrase);

        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult<UserTokens>> GetTokens([FromBody] AppUser user)
        {
            var result = await _authService.GetTokens(user);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpGet]
        [Route("authorize")]
        public async Task<ActionResult> LoginApp()
        {
            var result = await _authService.LoginApp();
            if (result != null)
            {
                return Redirect(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<UserTokens>> RefreshTokens(UserTokens usersToken)
        {
            var token = await _authService.RefreshTokens(usersToken);
            if (token != null)
                return Ok(token);
            return Unauthorized();
        }


        [HttpGet]
        [Route("validate")]
        public async Task<ActionResult<bool>> Validate()
        {
            string header = Request.Headers["Authorization"];
            var isValidated = await _authService.ValidateToken(header);
            if (isValidated)
                return Ok(isValidated);
            return Unauthorized();
        }
    }
}