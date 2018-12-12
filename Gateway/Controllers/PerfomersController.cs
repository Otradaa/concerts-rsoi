using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfomersController : ControllerBase
    {
        private readonly IGatewayService _gateway;

        public PerfomersController(IGatewayService gateway)
        {
            _gateway = gateway;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Perfomer> perfomers = await _gateway.GetPerfomers();
            return Ok(perfomers);
        }
    }
}