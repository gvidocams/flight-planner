using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Core.Validations;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public List<Airport> SearchAirports(string search)
        {
            search = search.ToLower().Trim();
            var airports = _context.Airports.Where(a => a.Country.ToLower().Trim().Contains(search) ||
                                                        a.City.ToLower().Trim().Contains(search) ||
                                                        a.AirportCode.ToLower().Trim().Contains(search));

            return airports.Distinct().ToList();
        }
    }
}