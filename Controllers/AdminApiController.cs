using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if (flight.From == null ||
                flight.To == null ||
                String.IsNullOrEmpty(flight.From.Country) ||
                String.IsNullOrEmpty(flight.From.City) ||
                String.IsNullOrEmpty(flight.From.AirportCode) ||
                String.IsNullOrEmpty(flight.To.Country) ||
                String.IsNullOrEmpty(flight.To.City) ||
                String.IsNullOrEmpty(flight.To.AirportCode) ||
                String.IsNullOrEmpty(flight.Carrier) ||
                String.IsNullOrEmpty(flight.DepartureTime) ||
                String.IsNullOrEmpty(flight.ArrivalTime))
            {
                return BadRequest();
            }

            if (flight.From.Country.ToLower().Trim() == flight.To.Country.ToLower().Trim() &&
                flight.From.City.ToLower().Trim() == flight.To.City.ToLower().Trim() &&
                flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim())
            {
                return BadRequest();
            }

            var departure = DateTime.Parse(flight.DepartureTime);
            var arrival = DateTime.Parse(flight.ArrivalTime);

            if(departure >= arrival)
            {
                return BadRequest();
            }

            if(FlightStorage.IsDuplicate(flight))
            {
                return Conflict();
            }

            flight = FlightStorage.AddFlight(flight);
            return Created("", flight);
        }
    }
}

