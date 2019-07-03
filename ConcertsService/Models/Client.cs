using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertsService.Models
{
    public class Client
    {
        public string appKey { get; set; }
        public string appSecret { get; set; }
    }

    public class ClientToken
    {
        public string Token { get; set; }
        public string ClientName { get; set; }
    }
}
