using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PerfomersServer.Models
{
    public class PerfomersServerContext : DbContext
    {
        public PerfomersServerContext (DbContextOptions<PerfomersServerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<PerfomersServer.Models.Perfomer> Perfomer { get; set; }
    }
}
