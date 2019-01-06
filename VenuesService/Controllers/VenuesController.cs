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
    public class VenuesController : ControllerBase
    {
        private readonly IVenuesRepository _repo;
        private readonly ILogger _logger;

        public VenuesController(IVenuesRepository repo, ILogger<VenuesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Venue> GetVenue()
        {
            _logger.LogInformation("-> requested GET /venues");
            return _repo.GetAllVenues();
        }

        // GET: api/Venues/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue([FromRoute] int id)
        {
            _logger.LogInformation("-> requested GET /venues/{id}", id);
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> model is not valid error");
                _logger.LogInformation("-> GET /venue/{id} returned BadRequest", id);
                return BadRequest(ModelState);
            }

            var venue = await _repo.GetVenue(id);

            if (venue == null)
            {
                _logger.LogInformation("-> GET /venue/{id} NotFound", id);
                return NotFound();
            }
            _logger.LogInformation("-> GET /venue/{id} returned Ok(venue)", id);
            return Ok(venue);
        }
    }
}