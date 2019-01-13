using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Concert
    {
        public int Id { get; set; }

        public int VenueId { get; set; }
        public int PerfomerId { get; set; }
        public DateTime Date { get; set; }


    }

    public class ConcertsCount
    {
        public List<Concert> concerts { get; set; }
        public int count { get; set; }
    }
}
