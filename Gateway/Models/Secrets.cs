using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    static public class Secrets
    {
        public static string appKey = "ImGateway";
        public static string appSecret = "ImSecretHi";
    }


    
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
