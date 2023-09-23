using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FlightTickets.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public Passenger? Passenger { get; set; }
        public int FlightId { get; set; }
        public Flight? Flight { get; set; } 
        [Display(Name = "Seat number")]
        public int SeatNumber { get; set;}
        public string AppUser { get; set; }

    }
}
