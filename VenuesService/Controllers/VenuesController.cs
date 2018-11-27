﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VenuesService.Models;

namespace VenuesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly VenuesContext _context;

        public VenuesController(VenuesContext context)
        {
            _context = context;
        }

        // GET: api/Venues
        [HttpGet]
        public IEnumerable<Venue> GetVenue()
        {
            return _context.Venue;
        }

        // GET: api/Venues/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venue = await _context.Venue.FindAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            return Ok(venue);
        }

        // PUT: api/Venues/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenue([FromRoute] int id, [FromBody] Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != venue.Id)
            {
                return BadRequest();
            }

            var context_venue = _context.Venue.Find(id);

            if (context_venue == null)
            {
                return NotFound();
            }

            _context.Entry(context_venue).State = EntityState.Modified;

            context_venue.Name = venue.Name;
            context_venue.Address = venue.Address;
            context_venue.Phone = venue.Phone;

            _context.Venue.Update(context_venue);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(id))
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

        // POST: api/Venues
        [HttpPost]
        public async Task<IActionResult> PostVenue([FromBody] Venue venue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Venue.Add(venue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenue", new { id = venue.Id }, venue);
        }

        // DELETE: api/Venues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();

            return Ok(venue);
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.Id == id);
        }
    }
}