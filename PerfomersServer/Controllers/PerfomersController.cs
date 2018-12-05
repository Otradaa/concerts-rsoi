using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PerfomersServer.Data;
using PerfomersServer.Models;

namespace PerfomersServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfomersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public PerfomersController(IRepository repository, ILogger<PerfomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Perfomers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfomer([FromRoute] int id)
        {
            _logger.LogInformation("-> requested GET /perfomers");
            if (!ModelState.IsValid)
            {
                _logger.LogError("-> model is not valid error");
                _logger.LogInformation("-> GET /perfomers/{id} returned BadRequest", id);
                return BadRequest(ModelState);
            }

            var perfomer = await _repository.GetPerfomer(id);

            if (perfomer == null)
            {
                _logger.LogInformation("-> GET /perfomers returned NotFound");
                return NotFound();
            }
            _logger.LogInformation("-> GET /perfomers returned Ok(perfomer)");
            return Ok(perfomer);
        }
    }
}