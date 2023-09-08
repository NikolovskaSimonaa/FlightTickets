using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlightTickets.Models;

namespace FlightTickets.Data
{
    public class FlightTicketsContext : DbContext
    {
        public FlightTicketsContext (DbContextOptions<FlightTicketsContext> options)
            : base(options)
        {
        }

        public DbSet<FlightTickets.Models.Flight> Flight { get; set; } = default!;

        public DbSet<FlightTickets.Models.Passenger>? Passenger { get; set; }

        public DbSet<FlightTickets.Models.Ticket>? Ticket { get; set; }
    }
}
