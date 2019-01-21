using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
        {

            try
            {
                var logged = await _authService.Login(user);
                if (logged == null)
                    return BadRequest();
                return Ok(logged);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("refreshtokens")]
        public async Task<ActionResult<UsersToken>> RefreshTokens(UsersToken usersToken)
        {

            try
            {
                var token = _authService.RefreshTokens(usersToken);
                if (token != null)
                    return Ok(token);
            }
            catch
            {
                return BadRequest();
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("validate")]
        public async Task<ActionResult<bool>> Validate()
        {

            try
            {
                string header = Request.Headers["Authorization"];

                bool flag = await _authService.ValidateToken(header);
                if (flag)
                    return Ok(flag);
            }
            catch
            {
                return BadRequest();
            }

            return Unauthorized();
        }



    }
}