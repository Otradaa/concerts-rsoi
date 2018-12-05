using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertsService.Models;
using ConcertsService.Data;
using Microsoft.Extensions.Logging;

namespace ConcertsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertRepository _repo;
        private readonly ILogger _logger;

        public ConcertsController(IConcertRepository repo, ILogger<ConcertsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: api/Concerts
        [HttpGet]
        public IEnumerable<Concert> GetConcert([FromQuery] int page = 1, [FromQuery] int size = 3)
        {
            _logger.LogInformation("-> requested GET /concerts");
            return _repo.GetAllConcerts(page, size);
        }

        // PUT: api/Concerts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcert([FromRoute] int id, [FromBody] Concert concert)
        {
            _logger.LogInformation("-> requested PUT /concerts");
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> PUT /concerts model is not valid");
                _logger.LogInformation("-> PUT /concerts returned BadRequest");
                return BadRequest(ModelState);
            }

            if (id != concert.Id)
            {
                _logger.LogInformation("-> PUT /concerts returned BadRequest");
                return BadRequest();
            }

            _repo.ChangeState(concert, EntityState.Modified);

            try
            {
                await _repo.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repo.ConcertExists(id))
                {
                    _logger.LogInformation("-> PUT /concerts returned NotFound");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("-> PUT /concerts returned NoContent");
            return NoContent();
        }

        // POST: api/Concerts
        [HttpPost]
        public async Task<IActionResult> PostConcert([FromBody] Concert concert)
        {
            _logger.LogInformation("-> requested POST /concerts");
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> POST /concerts model is not valid");
                _logger.LogInformation("-> POST /concerts returned BadRequest");
                return BadRequest(ModelState);
            }

            _repo.AddConcert(concert);
            await _repo.SaveChanges();

            _logger.LogInformation("-> POST /concerts returned Created with id = {id}", concert.Id);
            return CreatedAtAction("GetConcert", new { id = concert.Id }, concert);
        }
    }
}