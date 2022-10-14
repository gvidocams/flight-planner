using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Validations
{
    public class AirportCityValidator : IAirportValidator
    {
        public bool IsValid(Airport airport)
        {
            return !string.IsNullOrWhiteSpace(airport?.City);
        }
    }
}
