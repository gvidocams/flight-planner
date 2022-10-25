using System.Collections.Generic;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetCompleteFlightById(int id);
        bool Exists(Flight flight);
        List<Flight> GetFlights(string from, string to, string departureDate);
    }
}
