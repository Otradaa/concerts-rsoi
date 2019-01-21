using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static AuthService.Models.Account;

namespace AuthService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IToken tokenGenerator;

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


        [HttpGet, AllowAnonymous]
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
    }
}