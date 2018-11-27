using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private static HttpClient client = new HttpClient();
        // GET: api/Concerts
        [HttpGet]
        public async Task<IEnumerable<ConcertRequest>> Get()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:44343/api/concerts");
            //var client = new HttpClient();
            var response = await client.SendAsync(request);
            //List<Concert> concerts = JsonConvert.DeserializeObject<List<Concert>>(Convert.ToString(response.Content.re));
            List<Concert> concerts = await response.Content.ReadAsAsync<List<Concert>>();
            List<ConcertRequest> fullconcerts = new List<ConcertRequest>();
            foreach (var concert in concerts)
            {
                request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:44309/api/perfomers/" + concert.PerfomerId.ToString());
                response = await client.SendAsync(request);
                Perfomer perfomer = await response.Content.ReadAsAsync<Perfomer>();
                request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:44399/api/venues/" + concert.PerfomerId.ToString());
                response = await client.SendAsync(request);
                Venue venue = await response.Content.ReadAsAsync<Venue>();
                fullconcerts.Add(new ConcertRequest(concert.Id, perfomer.Name, venue.Name, venue.Address, concert.Date));
            }

            return fullconcerts;
        }

        // GET: api/Concerts/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Concerts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Concert concert)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:44343/api/concerts");
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            Concert _concert = await response.Content.ReadAsAsync<Concert>();

            if (response.IsSuccessStatusCode)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, _concert.Id);
                request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:44399/api/schedules");
                request.Content = new StringContent(JsonConvert.SerializeObject(schedule), Encoding.UTF8, "application/json");
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Concerts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Concert concert)
        {
            var request = new HttpRequestMessage(new HttpMethod("PUT"), "https://localhost:44343/api/concerts/"+id.ToString());
            request.Content = new StringContent(JsonConvert.SerializeObject(concert), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, id);
                request = new HttpRequestMessage(new HttpMethod("PUT"), "https://localhost:44399/api/schedules");
                request.Content = new StringContent(JsonConvert.SerializeObject(schedule), Encoding.UTF8, "application/json");
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
