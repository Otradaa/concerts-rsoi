using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AuthService.Models.Account;

namespace AuthService.Data
{
    public class UserContext : IdentityDbContext
    {
        new public DbSet<UserAccount> Users { get; set; }
        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }

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
