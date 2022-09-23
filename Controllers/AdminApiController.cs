using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController, Authorize]
    public class AdminApiController : ControllerBase
    {
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetFlight(id);
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
            if (Validate.HasNullValuesInFlight(flight) ||
                Validate.HasIdenticalAirportsFromAndTo(flight) ||
                Validate.DateTime(DateTime.Parse(flight.DepartureTime), DateTime.Parse(flight.ArrivalTime)))
            {
                return BadRequest(flight);
            }

            if (FlightStorage.IsDuplicate(flight))
            {
                return Conflict(flight);
            }

            flight = FlightStorage.AddFlight(flight);

            return Created("", flight);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            FlightStorage.DeleteFlight(id);

            return Ok();
        }
    }
}

