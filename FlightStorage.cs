using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = ++_id;
            _flights.Add(flight);
            return flight;
        }

        public static Flight GetFlight(int id)
        {
            return _flights.FirstOrDefault(f => f.Id == id);
        }

        public static void Clear()
        {
            _flights.Clear();
            _id = 0;
        }

        public static bool IsDuplicate(Flight flight)
        {
            foreach (Flight f in _flights)
            {
                if(f.From.Country == flight.From.Country &&
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
