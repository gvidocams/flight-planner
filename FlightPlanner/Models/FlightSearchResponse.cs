using FlightPlanner.Models;

namespace FlightPlanner.Core.Models
{
    public class FlightSearchResponse
    {
        public int page { get; set; }
        public int totalItems { get; set; }
        public FlightRequest[] items { get; set; }
    }
}
