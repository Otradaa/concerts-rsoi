using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AuthService.Models.Account;

namespace AuthService.Data
{
    public class AppRepository
    {
        private static List<AppAccount> TestUsers;
        public AppRepository()
        {
            TestUsers = new List<AppAccount>();
            TestUsers.Add(new AppAccount { AppId = "app", AppSecret = "secret" });
        }
        public AppAccount GetApp(string appId)
        {
            try
            {
                return TestUsers.First(user => user.AppId.Equals(appId));
            }
            catch
            {
                return null;
            }
        }
    }
}
