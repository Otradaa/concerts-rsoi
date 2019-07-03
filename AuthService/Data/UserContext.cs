using AuthService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Data
{
    public class UsersDb : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDb(DbContextOptions<UsersDb> options) : base(options) {
            Database.EnsureCreated();
        }
    }

    public class AppsDb : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public AppsDb(DbContextOptions<AppsDb> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }

    

}
