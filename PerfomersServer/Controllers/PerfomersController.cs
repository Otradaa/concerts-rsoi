using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfomersServer.Data;
using PerfomersServer.Models;

namespace PerfomersServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfomersController : ControllerBase
    {
        //private readonly PerfomersServerContext _context;
        private IRepository _repository;

        public PerfomersController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Perfomers
       /* [HttpGet]
        public IEnumerable<Perfomer> GetPerfomer()
        {
            return _context.Perfomer;
        }*/

        // GET: api/Perfomers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perfomer = await _repository.GetPerfomer(id);//_context.Perfomer.FindAsync(id);

            if (perfomer == null)
            {
                return NotFound();
            }

            return Ok(perfomer);
        }

        // PUT: api/Perfomers/5
       /* [HttpPut("{id}")]
        public async Task<IActionResult> PutPerfomer([FromRoute] int id, [FromBody] Perfomer perfomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != perfomer.Id)
            {
                return BadRequest();
            }

            _context.Entry(perfomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfomerExists(id))
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

        // POST: api/Perfomers
      /*  [HttpPost]
        public async Task<IActionResult> PostPerfomer([FromBody] Perfomer perfomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Perfomer.Add(perfomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerfomer", new { id = perfomer.Id }, perfomer);
        }
        */
        // DELETE: api/Perfomers/5
     /*   [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerfomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perfomer = await _context.Perfomer.FindAsync(id);
            if (perfomer == null)
            {
                return NotFound();
            }

            _context.Perfomer.Remove(perfomer);
            await _context.SaveChangesAsync();

            return Ok(perfomer);
        }*/

        /*private bool PerfomerExists(int id)
        {
            return _context.Perfomer.Any(e => e.Id == id);
        }*/
    }
}