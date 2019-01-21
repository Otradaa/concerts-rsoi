using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConcertsService.Models;
using ConcertsService.Auth;
using Microsoft.AspNetCore.Identity;

namespace ConcertsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IToken tokenGenerator;
*/
        public AuthController(IToken tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        [HttpPost, AllowAnonymous]
        public ActionResult<ClientToken> Login([FromBody] Client client)
        {
            if (ModelState.IsValid)
            {
                if (Secrets.appKey == client.appKey &&
                    Secrets.appSecret == client.appSecret)
                    return new ClientToken()
                    {
                        Token = tokenGenerator.GenerateToken(client.appKey),
                        ClientName = client.appKey
                    };
            }
            return BadRequest(ModelState);
        }
    }
}