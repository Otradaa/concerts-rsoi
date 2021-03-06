﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Data;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [Authorize(Roles = "user")]
    [Route("api/users")]
    [ApiController]
    public class AccountController : Controller
    {
        /*   private readonly IToken tokenGenerator;

           private readonly UserManager<UserAccount> userManager;
           private readonly SignInManager<UserAccount> signInManager;
           private readonly RoleManager<IdentityRole> roleManager;

           private readonly AppRepository appManager;

           public AccountController(

               IToken tokenGenerator,
               UserManager<UserAccount> userManager,
               SignInManager<UserAccount> signInManager,
               RoleManager<IdentityRole> roleManager
           )
           {

               this.tokenGenerator = tokenGenerator;
               this.userManager = userManager;
               this.signInManager = signInManager;
               this.roleManager = roleManager;

               if (!roleManager.Roles.Any())
               {
                   roleManager.CreateAsync(new IdentityRole("app"));
                   roleManager.CreateAsync(new IdentityRole("user"));
               }

               appManager = new AppRepository();



           }
           */


        
        private readonly UsersDb _usersDb;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private AppUser appUser;
        public AccountController(UsersDb usersDb, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _usersDb = usersDb;
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
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;

            var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;

            await _usersDb.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserTokens>> Login([FromBody] User _user)
        {
            string username = _user.Username;
            string password = _user.Password;
            var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            if (user == null || !_passwordHasher.VerifyIdentityV3Hash(password, user.Password)) return BadRequest();

            var usersClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
              //  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
            var refreshToken = _tokenService.GenerateRefreshToken(usersClaims);

            user.RefreshToken = refreshToken;
            await _usersDb.SaveChangesAsync();

            return new UserTokens
            {
                token = jwtToken,
                refreshToken = refreshToken
            };
        }

        ////////////////////////////////////////////////////////////////
        /* [HttpGet, AllowAnonymous]
         [ProducesResponseType((int)HttpStatusCode.NotFound)]
         [ProducesResponseType(typeof(List<IdentityUser>), (int)HttpStatusCode.OK)]
         public async Task<IActionResult> GetUsers()
         {
             if (!userManager.Users.Any())
             {
                 var user = new UserAccount { UserName = "First" };
                 var result = await userManager.CreateAsync(user, "Password");
                 await userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
                 var app = new UserAccount { UserName = "Second" };
                 var result1 = await userManager.CreateAsync(app, "Password");
                 var sssss = await userManager.AddClaimAsync(app, new Claim("userName", app.UserName));
                 //  userManager.AddClaimAsync(user, new Claim("role", "user"));
             }
             return Ok(userManager.Users);
         }

         [HttpPost, AllowAnonymous]
         public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
         {
             if (ModelState.IsValid)
             {
                 var result = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

                 if (result.Succeeded)
                 {
                     var acccount = await userManager.FindByNameAsync(user.Username);

                     if (acccount != null)
                     {
                         var jwt = tokenGenerator.GenerateRefreshToken(acccount.Id, acccount.UserName);
                         acccount.Token = jwt;
                         await userManager.UpdateAsync(acccount);
                         return new UsersToken()
                         {
                             RefreshToken = jwt,
                             AccessToken = tokenGenerator.GenerateAccessToken(acccount.Id, acccount.UserName),
                             UserName = user.Username
                         };

                     }

                 }
                 return Forbid();

             }
             return BadRequest(ModelState);
         }


         [HttpPost, Route("refreshtokens"),AllowAnonymous]
         public async Task<ActionResult<UsersToken>> RefreshTokens(UsersToken usersToken)
         {
             //if (this.User.Identity.IsAuthenticated)
             if (signInManager.IsSignedIn(User))
             {

                 //   string token = Request.Headers["Authorization"].ToString().Remove(0,7);
                 var user = await userManager.FindByNameAsync(usersToken.UserName);

                 if (user != null && user.Token == usersToken.RefreshToken)
                 {
                     if (tokenGenerator.ValidateToken(usersToken.RefreshToken))
                     {
                         var jwt = tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                         user.Token = jwt;
                         await userManager.UpdateAsync(user);
                         return new UsersToken()
                         {
                             AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                             RefreshToken = jwt,
                             UserName = user.UserName
                         };
                     }
                 }
             }
             return BadRequest();
         }

         [HttpGet, Route("refreshtokens")]
         public async Task<ActionResult<UsersToken>> RefreshTokens()
         {
             if (this.User.Identity.IsAuthenticated)
             {

                 string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
                 var user = await userManager.FindByNameAsync(User.Identity.Name//usersToken.UserName);

                 if (user != null && user.Token == token //usersToken.RefreshToken)
                 {
                     if (tokenGenerator.ValidateToken(token))
                     {
                         var jwt = tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                         user.Token = jwt;
                         await userManager.UpdateAsync(user);
                         return new UsersToken()
                         {
                             AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                             RefreshToken = jwt,
                             UserName = user.UserName
                         };
                     }
                 }
             }
             return BadRequest();
         }



         [HttpGet, Route("validate")]
         public async Task<ActionResult<bool>> Validate()
         {
             if (this.User.Identity.IsAuthenticated)
             {

                 string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
                 var user = await userManager.FindByNameAsync(User.Identity.Name/*usersToken.UserName);

                 if (tokenGenerator.ValidateToken(token))
                 {
                     return Ok(true);
                 }
             }
             return Unauthorized();
         }

         [HttpGet, Route("authapp")]
         public async Task<ActionResult> AutorizeApplication([FromQuery]string client_id = "app")
         {
             if (this.User.Identity.IsAuthenticated)
             {
                 var acccount = appManager.GetApp(client_id);

                 if (acccount != null)
                 {
                     //   var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                     acccount.AutorizedUsers.Add(this.User.Identity.Name);
                 }
                 return Ok();
             }
             return BadRequest();
         }*/

    }



}
