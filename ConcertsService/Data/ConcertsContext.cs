using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConcertsService.Models
{
    public class ConcertsContext : DbContext
    {
        public ConcertsContext (DbContextOptions<ConcertsContext> options)
            : base(options)
        {
        }

        public DbSet<ConcertsService.Models.Concert> Concert { get; set; }
    }
}
