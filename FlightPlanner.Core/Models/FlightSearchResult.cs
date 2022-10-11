namespace FlightPlanner.Core.Models
{
    public class FlightSearchResult
    {
        public int page { get; set; }
        public int totalItems { get; set; }
        public Flight[] items { get; set; }
    }
}
