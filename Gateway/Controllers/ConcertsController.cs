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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private readonly IGatewayService _gateway;
        private readonly ILogger _logger;

        public ConcertsController(IGatewayService gateway, ILogger<ConcertsController> logger)
        {
            _gateway = gateway;
            _logger = logger;
        }

        // GET: api/Concerts
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int pageSize)
        {
            _logger.LogInformation("-> requested GET /concerts?page={page}&pageSize={pageSize}", page, pageSize);
            ConcertsCount concertsCount = await _gateway.GetConcerts(page, pageSize);
            List<Concert> concerts = concertsCount.concerts;
            int count = concertsCount.count;
            if (concerts != null)
            {
                List<ConcertRequest> fullconcerts = new List<ConcertRequest>();
                foreach (var concert in concerts)
                {
                    _logger.LogInformation($"-> request to GET /perfomers/{concert.PerfomerId}");
                    Perfomer perfomer = await _gateway.GetPerfomerById(concert.PerfomerId);
                    if (perfomer != null)
                    {
                        _logger.LogInformation("-> request to GET /venues/{0}", concert.VenueId);
                        Venue venue = await _gateway.GetVenueById(concert.VenueId);
                        //validate?
                        if (venue != null)
                            fullconcerts.Add(new ConcertRequest(concert.Id, perfomer.Name,
                                venue.Name, venue.Address, concert.Date));
                    }
                }
                _logger.LogInformation("-> GET /concerts?page={0}&pageSize={1} returned {2} concert(s)", 
                    page, pageSize, fullconcerts.Count);
                ConcertsPage concertsPage = new ConcertsPage(count,page, pageSize,fullconcerts);
                return Ok(concertsPage);
            }
            _logger.LogInformation("-> GET /concerts : no concerts found");
            return NoContent();
        }

        // POST: api/Concerts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Concert concert)
        {
            _logger.LogInformation("-> requested POST /concerts");
            _logger.LogInformation("-> request to POST concertsService/concerts");
            var response = await _gateway.PostConcert(concert);
            if (response.IsSuccessStatusCode)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, concert.Id);
                _logger.LogInformation("-> request to POST schedulesService/schedules");
                bool success = await _gateway.PostSchedule(schedule);
                if (success)
                {
                    _logger.LogInformation("-> POST /concerts returned new concert with id {0}",
                        concert.Id);
                    return CreatedAtAction("Get", new { id = concert.Id }, concert);
                }
            }
            _logger.LogInformation("-> POST /concerts returned BadRequest");
            return BadRequest(response.ReasonPhrase);
        }

        // PUT: api/Concerts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Concert concert)
        {
            _logger.LogInformation("-> requested PUT /concerts/{id}", id);
            _logger.LogInformation("-> request to PUT concertsService/concerts");
            var response = await _gateway.PutConcert(id, concert);
            if (response.IsSuccessStatusCode)
            {
                Schedule schedule = new Schedule(concert.VenueId, concert.Date, id);
                _logger.LogInformation("-> request to PUT schedulesService/schedules");
                bool success = await _gateway.PutSchedule(schedule);
                if (success)
                {
                    _logger.LogInformation("-> PUT /concerts returned NoContent");
                    return NoContent();
                }
            }
            _logger.LogInformation("-> PUT /concerts returned BadRequest");
            return BadRequest(response.ReasonPhrase);
        }
    }
}
