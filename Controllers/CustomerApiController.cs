using FlightPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirports(string search)
        {
            return Ok(FlightStorage.FindAirports(search));
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult GetFlights(FlightSearchRequest req)
        {
            if(req.From == req.To)
            {
                return BadRequest();
            }

            var flights = FlightStorage.FindFlights(req.From, req.To);

            var result = new FlightSearchResult() { page = 0, totalItems = flights.Length, items = flights };

            return Ok(result);
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlightById(int id)
        {
            var flight = FlightStorage.GetFlight(id);

            if(flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
