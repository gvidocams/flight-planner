using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private static readonly object dbLock = new object();
        private IFlightService _flightService;
        private IAirportService _airportService;
        private IMapper _mapper;

        public CustomerApiController(
            IFlightService flightService,
            IAirportService airportService,
            IMapper mapper)
        {
            _flightService = flightService;
            _airportService = airportService;
            _mapper = mapper;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult GetAirports(string search)
        {
            var airports = _airportService.SearchAirports(search);

            var response = airports.ConvertAll(a => _mapper.Map<AirportRequest>(a)).ToArray();

            return Ok(response);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult GetFlights(FlightSearchRequest req)
        {
            lock (dbLock)
            {
                if (req.From == req.To)
                {
                    return BadRequest();
                }

                var flights = _flightService.GetFlights(req.From, req.To, req.DepartureDate);

                var mappedFlights = flights.ConvertAll(f => _mapper.Map<FlightRequest>(f));

                var response = new FlightSearchResponse()
                {
                    page = 0,
                    totalItems = mappedFlights.Count,
                    items = mappedFlights.ToArray()
                };

                return Ok(response);
            }
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlightById(int id)
        {
            var flight = _flightService.GetCompleteFlightById(id);

            if (flight == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<FlightRequest>(flight);

            return Ok(response);
        }
    }
}
