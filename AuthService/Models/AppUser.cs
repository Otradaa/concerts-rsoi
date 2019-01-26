using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        public string client_id { get; set; }
        public string appSecret { get; set; }
        public string code { get; set; }
        public string refreshToken { get; set; }
    }
}
