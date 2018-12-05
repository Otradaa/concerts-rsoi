using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public int? ConcertId { get; set; }
        public int VenueId { get; set; }

        public DateTime Date { get; set; }

        public Schedule()
        { 
        }
        public Schedule(int vid, DateTime date, int? cid)
        {
            VenueId = vid;
            Date = date;
            ConcertId = cid;
        }
    }
}
