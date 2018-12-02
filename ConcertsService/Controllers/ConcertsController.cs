using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertsService.Models;
using ConcertsService.Data;

namespace ConcertsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertRepository _repo;

        public ConcertsController(IConcertRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Concerts
        [HttpGet]
        public IEnumerable<Concert> GetConcert([FromQuery] int page = 1, [FromQuery] int size = 2)
        {
            return _repo.GetAllConcerts(page, size);
        }

        // GET: api/Concerts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConcert([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concert = await _repo.GetConcert(id);

            if (concert == null)
            {
                return NotFound();
            }

            return Ok(concert);
        }

        // PUT: api/Concerts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcert([FromRoute] int id, [FromBody] Concert concert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != concert.Id)
            {
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Concerts
        [HttpPost]
        public async Task<IActionResult> PostConcert([FromBody] Concert concert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.AddConcert(concert);
            await _repo.SaveChanges();

            return CreatedAtAction("GetConcert", new { id = concert.Id }, concert);
        }

        // DELETE: api/Concerts/5
      /*  [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcert([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concert = await _context.Concert.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }

            _context.Concert.Remove(concert);
            await _context.SaveChangesAsync();

            return Ok(concert);
        }*/

        
    }
}