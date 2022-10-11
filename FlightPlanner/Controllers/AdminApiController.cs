using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController, Authorize]
    public class AdminApiController : ControllerBase
    {
        private static readonly object dbLock = new object();

        private readonly IFlightService _flightService;

        public AdminApiController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetCompleteFlightById(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);

        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(Flight flight)
        {
            /*
            if (Validate.IsValidFlight(flight))
            {
                return BadRequest(flight);
            }
            */
            /*
            if (Validate.IsDuplicateFlight(flights, flight))
            {
                return Conflict(flight);
            }
            */
            _flightService.Create(flight);
            return Created("", flight);

        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _flightService.GetById(id);

            if (flight != null)
            {
                _flightService.Delete(flight);
            }

            return Ok();

        }
    }
}

