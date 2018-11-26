using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertsService.Models
{
    public class Concert
    {
        public int Id { get; set; }

        public int VenueId { get; set; }
        public int PerfomerId { get; set; }
        public DateTime Date { get; set; }

    }
}
