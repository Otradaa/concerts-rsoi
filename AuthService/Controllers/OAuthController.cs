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

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly AppRepository appManager;

        private readonly UserManager<UserAccount> userManager;
        private readonly SignInManager<UserAccount> signInManager;
        private readonly IToken _tokenGenerator;

        private readonly RoleManager<IdentityRole> roleManager;

        public OAuthController(IToken tokenGenerator,
            UserManager<UserAccount> userManager,
           SignInManager<UserAccount> signInManager,
           RoleManager<IdentityRole> roleManager)
        {
            _tokenGenerator = tokenGenerator;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            appManager = new AppRepository();
        }
        // Client : client_id, appKey, appSecret, RedirectionURI

        // адрес формы авторизации

        // кнопка АПИ -- запрос формы авторизации
        [HttpGet]
        public async Task<ActionResult<string>> Authorize([FromQuery]string client_id = "123", [FromQuery]string redirect_uri = "https://localhost:44358/api/account", [FromQuery]string response_type = "code")
        {

            var acccount = appManager.GetApp(client_id);//await userManager.FindByNameAsync(client_id);

            if (acccount != null) //если приложение зарегистрировано 
            {

                if (!User.Identity.IsAuthenticated || !acccount.AutorizedUsers.Contains(this.User.Identity.Name))
                    return RedirectPermanent("https://localhost:44358/Auth.html?client_id=app");

                acccount.AuthCode = GetAuthCode();
                // await userManager.UpdateAsync(acccount);
                Response.Headers.Add("Content-Type", "application/json");
                return RedirectPermanent($"{redirect_uri}?code={acccount.AuthCode}");
            }
            return BadRequest();          
        }

        [HttpGet]
        [Route("token")]
        public async Task<ActionResult<UsersToken>> GetToken([FromQuery]string code, [FromQuery]string client_secret = "secret", [FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account")
        {

            var acccount = appManager.GetApp(client_id);

            if (acccount != null)
            {
                if (acccount.AppSecret == client_secret && acccount.AuthCode == code)
                {
                    var user = await userManager.FindByNameAsync(User.Identity.Name);
                    if (user != null)
                    {
                        var jwt = _tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                        user.Token = jwt;
                        await userManager.UpdateAsync(user);
                        return new UsersToken()
                        {
                            AccessToken = _tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                            RefreshToken = jwt,
                            UserName = user.UserName
                        };
                    }
                }
            }
            return BadRequest();
        }
        private string GetAuthCode()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next().ToString();
        }
        // получение данных клиента валидация и выдача кода авторизации

        // получение кода авторизации и выдача токена

        // запросы на ресурсы с валидацией токена
    }
}