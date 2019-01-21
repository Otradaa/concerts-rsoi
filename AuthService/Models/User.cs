using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UsersToken
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
    }
}
