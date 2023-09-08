using FlightTickets.Models;

namespace FlightTickets.ViewModels
{
    public class SearchPassengerViewModel
    {
        public IList<Passenger> Passengers { get; set; }
        public string SearchPassenger { get; set; }
    }
}
