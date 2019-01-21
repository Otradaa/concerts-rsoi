using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Data
{
    public class UserRepository
    {
        public interface IUsersRepository
        {
            Task<List<User>> GetUsers();
        }
    }
}
