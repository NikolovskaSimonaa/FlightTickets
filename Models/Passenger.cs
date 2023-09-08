using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FlightTickets.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Passport Id")]
        public string PassportIdentificationNumber { get; set; }
        public ICollection<Ticket>? MyTickets { get; set; }

    }
}
