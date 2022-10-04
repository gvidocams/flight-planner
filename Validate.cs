using System;
using System.Collections.Generic;

namespace FlightPlanner
{
    public static class Validate
    {
        public static readonly object dbLock = new object();

        public static bool IsValidFlight(Flight flight)
        {
            return Validate.HasNullValuesInFlight(flight) ||
                   Validate.HasIdenticalAirportsFromAndTo(flight) ||
                   Validate.Datetime(DateTime.Parse(flight.DepartureTime), DateTime.Parse(flight.ArrivalTime));
        }
        public static bool HasNullValuesInFlight(Flight flight)
        {
            return flight.From == null ||
                   flight.To == null ||
                   String.IsNullOrEmpty(flight.From.Country) ||
                   String.IsNullOrEmpty(flight.From.City) ||
                   String.IsNullOrEmpty(flight.From.AirportCode) ||
                   String.IsNullOrEmpty(flight.To.Country) ||
                   String.IsNullOrEmpty(flight.To.City) ||
                   String.IsNullOrEmpty(flight.To.AirportCode) ||
                   String.IsNullOrEmpty(flight.Carrier) ||
                   String.IsNullOrEmpty(flight.DepartureTime) ||
                   String.IsNullOrEmpty(flight.ArrivalTime);
        }

        public static bool HasIdenticalAirportsFromAndTo(Flight flight)
        {
            return flight.From.Country.ToLower().Trim() == flight.To.Country.ToLower().Trim() &&
                   flight.From.City.ToLower().Trim() == flight.To.City.ToLower().Trim() &&
                   flight.From.AirportCode.ToLower().Trim() == flight.To.AirportCode.ToLower().Trim();
        }

        public static bool Datetime(DateTime departure, DateTime arrival)
        {
            return departure >= arrival;
        }

        public static bool IsDuplicateFlight(List<Flight> flights, Flight flight)
        {
            foreach (Flight f in flights)
            {
                if (f.From.Country == flight.From.Country &&
                    f.From.City == flight.From.City &&
                    f.From.AirportCode == flight.From.AirportCode &&
                    f.To.Country == flight.To.Country &&
                    f.To.City == flight.To.City &&
                    f.To.AirportCode == flight.To.AirportCode &&
                    f.Carrier == flight.Carrier &&
                    f.DepartureTime == flight.DepartureTime &&
                    f.ArrivalTime == flight.ArrivalTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
