using System.Collections;
using System.Collections.Generic;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.Validations;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController, Authorize]
    public class AdminApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IEnumerable<IFlightValidator> _flightValidators;
        private readonly IEnumerable<IAirportValidator> _airportValidators;
        private readonly IMapper _mapper;

        public AdminApiController(IFlightService flightService, 
            IEnumerable<IAirportValidator> airportValidators, 
            IEnumerable<IFlightValidator> flightValidators,
            IMapper mapper)
        {
            _flightService = flightService;
            _airportValidators = airportValidators;
            _flightValidators = flightValidators;
            _mapper = mapper;
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

            var response = _mapper.Map<FlightRequest>(flight);

            return Ok(response);

        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(FlightRequest request)
        {
            var flight = _mapper.Map<Flight>(request);
            if (!_flightValidators.All(f => f.IsValid(flight)) ||
                !_airportValidators.All(a => a.IsValid(flight?.From)) ||
                !_airportValidators.All(a => a.IsValid(flight?.To)))
            {
                return BadRequest(request);
            }
            
            if (_flightService.Exists(flight))
            {
                return Conflict(request);
            }

            var result = _flightService.Create(flight);

            if (result.Success)
            {
                request = _mapper.Map<FlightRequest>(flight);

                return Created("", request);
            }

            return Problem(result.FormatErrors);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _flightService.GetById(id);

            if (flight != null)
            {
                var result = _flightService.Delete(flight);

                if (result.Success)
                {
                    return Ok();
                }

                return Problem(result.FormatErrors);
            }

            return Ok();
        }
    }
}

