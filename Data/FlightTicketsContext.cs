using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FlightTickets.Areas.Identity.Data;
using FlightTickets.Models;

namespace FlightTickets.Data
{
    public class FlightTicketsContext : IdentityDbContext<FlightTicketsUser>
    {
        public FlightTicketsContext (DbContextOptions<FlightTicketsContext> options)
            : base(options)
        {
        }

        public DbSet<FlightTickets.Models.Flight> Flight { get; set; } = default!;

        public DbSet<FlightTickets.Models.Passenger>? Passenger { get; set; }

        public DbSet<FlightTickets.Models.Ticket>? Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
