using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VenuesService.Models
{
    public class Schedule
    {
        public int Id { get; set; }
       
        public int? ConcertId { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [Display(Name = "Not Available Date")]
        public DateTime Date { get; set; }

    }
}
