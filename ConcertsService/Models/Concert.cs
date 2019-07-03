using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ConcertsService.Models
{
    public class Concert
    {
        public int Id { get; set; }

        [Required]
        public int VenueId { get; set; }
        [Required]
        public int PerfomerId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/2000", "1/1/2100",
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime Date { get; set; }
    }

    public class ConcertsCount
    {
        public List<Concert> concerts { get; set; }
        public int count { get; set; }
    }
}
