using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace PerfomersServer.Models
{
    public class PerfomersServerContext : DbContext
    {
        public PerfomersServerContext (DbContextOptions<PerfomersServerContext> options)
            : base(options)
        {
           // Database.SetInitializer<PerfomersServerContext>();
        }

        public DbSet<PerfomersServer.Models.Perfomer> Perfomer { get; set; }
    }
}
