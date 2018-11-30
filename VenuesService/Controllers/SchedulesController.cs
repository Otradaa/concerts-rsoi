using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenuesService.Data;
using VenuesService.Models;

namespace VenuesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IVenuesRepository _repo;

        public SchedulesController(IVenuesRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Schedules
        /* [HttpGet]
         public IEnumerable<Schedule> GetSchedule()
         {
             return _context.Schedule;
         }*/

        // GET: api/Schedules/5
        /*   [HttpGet("{id}")]
           public async Task<IActionResult> GetSchedule([FromRoute] int id)
           {
               if (!ModelState.IsValid)
               {
                   return BadRequest(ModelState);
               }

               var schedule = await _context.Schedule.FindAsync(id);

               if (schedule == null)
               {
                   return NotFound();
               }

               return Ok(schedule);
           }*/

        // PUT: api/Schedules
        [HttpPut]
        public async Task<IActionResult> PutSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*if (id != schedule.Id)
            {
                return BadRequest();
            }*/

            var context_schedule = _repo.FirstSchedule(schedule.ConcertId);

            if (context_schedule == null)
            {
                return NotFound();
            }

            _repo.ChangeState(context_schedule, EntityState.Modified);
            /*context_schedule.VenueId = schedule.VenueId;
            context_schedule.Date = schedule.Date;

            _context.Schedule.Update(context_schedule);*/

            try
            {
                await _repo.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repo.ScheduleExists(context_schedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    /*    public async Task<IActionResult> PutSchedule([FromRoute] int id, [FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != schedule.Id)
            {
                return BadRequest();
            }

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Schedules
        [HttpPost]
        public async Task<IActionResult> PostSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.AddSchedule(schedule);
            await _repo.SaveChanges();

            return CreatedAtAction("GetSchedule", new { id = schedule.Id }, schedule);
        }

        // DELETE: api/Schedules/5
      /*  [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }*/

    }
}