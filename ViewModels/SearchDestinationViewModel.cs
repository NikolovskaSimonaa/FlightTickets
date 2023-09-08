using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using FlightTickets.Models;

namespace FlightTickets.ViewModels
{
    public class SearchDestinationViewModel
    {
        public IList<Flight> Flights { get; set; }
        public string SearchDestination { get; set; }
        public string SearchDeparture { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
