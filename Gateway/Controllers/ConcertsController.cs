using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private readonly IGatewayService gateway;
        public ConcertsController(IGatewayService _gateway)
        {
            gateway = _gateway;
        }
        //private static HttpClient client = new HttpClient();
        // GET: api/Concerts
        [HttpGet]
        public async Task<IEnumerable<ConcertRequest>> Get([FromQuery] int page=1, [FromQuery] int pageSize=2)
        {
            List<Concert> concerts = await gateway.GetConcerts(page, pageSize);
            List<ConcertRequest> fullconcerts = new List<ConcertRequest>();
            foreach (var concert in concerts)
            {
                Perfomer perfomer = await gateway.GetPerfomerById(concert.PerfomerId);
                Venue venue = await gateway.GetVenueById(concert.VenueId);
                //validate?
                if (venue != null && venue != null)
                    fullconcerts.Add(new ConcertRequest(concert.Id, perfomer.Name, 
                        venue.Name, venue.Address, concert.Date));
            }

            return fullconcerts;
        }

        // GET: api/Concerts/5
       /* [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/Concerts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Concert concert)
        {
            int id;
            bool success;
            (success, id) = await gateway.PostConcert(concert);
            if (success)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, id);
                success = await gateway.PostSchedule(schedule);
                if (success)
                    return Ok();
            }
            return BadRequest();
        }

        // PUT: api/Concerts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Concert concert)
        {
            bool success = await gateway.PutConcert(id, concert);
            if (success)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, id);
                success = await gateway.PutSchedule(schedule);
                if (success)
                    return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/ApiWithActions/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
