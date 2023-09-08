using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace FlightTickets.Models
{
    public class Flight
    {
        [Display(Name = "Flight Number")]
        public int Id { get; set; }
        public DateTime Date { get; set; } //Departure time
        public string Departure { get; set; }
        public string Destination { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
