using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VenuesService.Data;
using VenuesService.Models;

namespace VenuesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IVenuesRepository _repo;
        private readonly ILogger _logger;

        public SchedulesController(IVenuesRepository repo, ILogger<SchedulesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule([FromRoute] int id)
        {
            _logger.LogInformation("-> requested GET /schedules/{id}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> model is not valid error");
                _logger.LogInformation("-> GET /schedules/{id} returned BadRequest", id);
                return BadRequest(ModelState);
            }

            var schedule = await _repo.GetSchedule(id);

            if (schedule == null)
            {
                _logger.LogInformation("-> GET /schedules/{id} NotFound", id);
                return NotFound();
            }
            _logger.LogInformation("-> GET /schedules/{id} returned Ok(schedule)", id);
            return Ok(schedule);
        }

        // PUT: api/Schedules
        [HttpPut]
        public async Task<IActionResult> PutSchedule([FromBody] Schedule schedule)
        {
            _logger.LogInformation("-> requested PUT /schedules");
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> PUT / schedules model is not valid");
                _logger.LogInformation("-> PUT /schedules returned BadRequest");
                return BadRequest(ModelState);
            }

            var context_schedule = _repo.FirstSchedule(schedule.ConcertId);

            if (context_schedule == null)
            {
                _logger.LogInformation("-> PUT /schedules returned NotFound");
                return NotFound();
            }

            _repo.ChangeState(context_schedule, schedule, EntityState.Modified);

            try
            {
                await _repo.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repo.ScheduleExists(context_schedule.Id))
                {
                    _logger.LogInformation("-> PUT /schedules returned NotFound");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("-> PUT /schedules returned NoContent");
            return NoContent();
        }

        // POST: api/Schedules
        [HttpPost]
        public async Task<IActionResult> PostSchedule([FromBody] Schedule schedule)
        {
            _logger.LogInformation("-> requested POST /schedules");
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> POST /schedules model is not valid");
                _logger.LogInformation("-> POST /schedules returned BadRequest");
                return BadRequest(ModelState);
            }

            _repo.AddSchedule(schedule);
            await _repo.SaveChanges();
            _logger.LogInformation("-> POST /schedules returned Created with id = {id}", schedule.Id);
            return CreatedAtAction("GetSchedule", new { id = schedule.Id }, schedule);
        }
    }
}