using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        public DateTime Date { get; set; }

        public Schedule(int vid, DateTime date)
        {
            VenueId = vid;
            Date = date;
        }
    }
}
