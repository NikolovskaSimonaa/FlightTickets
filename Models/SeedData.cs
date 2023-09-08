using Microsoft.EntityFrameworkCore;
using FlightTickets.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FlightTickets.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FlightTicketsContext(serviceProvider.GetRequiredService<DbContextOptions<FlightTicketsContext>>()))
            {
                if (context.Ticket.Any() || context.Passenger.Any() || context.Flight.Any())
                {
                    return;
                }
                context.Flight.AddRange(
                    new Flight { Date = DateTime.Parse("2023-12-12 18:00:00"), Departure = "Skopje, Macedonia", Destination = "Rome, Italy" },
                    new Flight { Date = DateTime.Parse("2023-12-15 11:00:00"), Departure = "Rome, Italy", Destination = "Skopje, Macedonia" },
                    new Flight { Date = DateTime.Parse("2023-12-15 12:00:00"), Departure = "Skopje, Macedonia", Destination = "Vienna, Austria" },
                    new Flight { Date = DateTime.Parse("2023-12-19 12:00:00"), Departure = "Vienna, Austria", Destination = "Skopje, Macedonia" }
                );
                context.SaveChanges();
                context.Passenger.AddRange(
                   new Passenger { Name = "Simona Nikolovska", DateOfBirth = DateTime.Parse("2002-5-10"), PassportIdentificationNumber = "ABCD1234" },
                   new Passenger { Name = "Marija Nikolovska", DateOfBirth = DateTime.Parse("1998-8-23"), PassportIdentificationNumber = "BBCD1234" }
               );
                context.SaveChanges();
                context.Ticket.AddRange(
                   new Ticket { PassengerId = 1, FlightId = 1, SeatNumber = 58 },
                   new Ticket { PassengerId = 2, FlightId = 1, SeatNumber = 59 },
                   new Ticket { PassengerId = 1, FlightId = 2, SeatNumber = 43 },
                   new Ticket { PassengerId = 2, FlightId = 2, SeatNumber = 42 }
               );
                context.SaveChanges();
            }
        }
    }
            
}

