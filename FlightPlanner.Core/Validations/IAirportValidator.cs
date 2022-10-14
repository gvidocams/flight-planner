using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Validations
{
    public interface IAirportValidator
    {
        bool IsValid(Airport airport);
    }
}
