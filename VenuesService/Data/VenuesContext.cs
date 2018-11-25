using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VenuesService.Models
{
    public class VenuesContext : DbContext
    {
        public VenuesContext (DbContextOptions<VenuesContext> options)
            : base(options)
        {
        }

        public DbSet<VenuesService.Models.Schedule> Schedule { get; set; }
        public DbSet<VenuesService.Models.Venue> Venue { get; set; }
    }
}
