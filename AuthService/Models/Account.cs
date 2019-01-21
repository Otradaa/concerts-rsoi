using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class Account
    {
        public class UserAccount : IdentityUser
        {
            public string Token { get; set; }

        }

        public class AppAccount
        {
            public string AppId;
            public string AppSecret;
            public string AuthCode;
            public List<string> AutorizedUsers = new List<string>();
        }
    }
}
