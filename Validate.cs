using System;

namespace FlightPlanner
{
    public static class Validate
    {
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

        public static bool DateTime(DateTime departure, DateTime arrival)
        {
            return departure >= arrival;
        }
    }
}
