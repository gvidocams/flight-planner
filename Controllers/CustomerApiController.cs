using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private static readonly object dbLock = new object();
        private readonly FlightPlannerDbContext _context;

        public CustomerApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirports(string search)
        {
            var searchedList = new List<Airport>();
            var fixedKeyword = search.ToLower().Trim();

            var flights = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .ToList();

            foreach (Flight f in flights)
            {
                if (f.To.Country.ToLower().Contains(fixedKeyword) ||
                    f.To.City.ToLower().Contains(fixedKeyword) ||
                    f.To.AirportCode.ToLower().Contains(fixedKeyword))
                {
                    searchedList.Add(f.To);
                }

                if (f.From.Country.ToLower().Contains(fixedKeyword) ||
                    f.From.City.ToLower().Contains(fixedKeyword) ||
                    f.From.AirportCode.ToLower().Contains(fixedKeyword))
                {
                    searchedList.Add(f.From);
                }
            }

            return Ok(searchedList.ToArray());
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult GetFlights(FlightSearchRequest req)
        {
            if (req.From == req.To)
            {
                return BadRequest();
            }

            lock (dbLock)
            {
                var flights = _context.Flights
                    .Include(f => f.From)
                    .Include(f => f.To)
                    .ToList()
                    .Where(f => f.From.AirportCode == req.From && f.To.AirportCode == req.To)
                    .ToArray();

                var result = new FlightSearchResult() { page = 0, totalItems = flights.Length, items = flights };

                return Ok(result);
            }
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlightById(int id)
        {
            var flight = _context.Flights
            .Include(f => f.From)
            .Include(f => f.To)
            .FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
