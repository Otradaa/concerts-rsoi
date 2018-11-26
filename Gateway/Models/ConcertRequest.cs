using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class ConcertRequest
    {
        public int Id { get; set; }

        public string PerfomerName { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public DateTime Date { get; set; }

        public ConcertRequest(int id, string pname, string vname, string vaddress, DateTime date)
        {
            Id = id;
            PerfomerName = pname;
            VenueName = vname;
            VenueAddress = vaddress;
            Date = date;
        }
    }
}
