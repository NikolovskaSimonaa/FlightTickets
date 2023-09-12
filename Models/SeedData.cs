using Microsoft.EntityFrameworkCore;
using FlightTickets.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using FlightTickets.Areas.Identity.Data;

namespace FlightTickets.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<FlightTicketsUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            FlightTicketsUser admin = await UserManager.FindByEmailAsync("simona@tickets.com");
            if (admin == null)
            {
                var Admin = new FlightTicketsUser();
                Admin.Email = "simona@tickets.com";
                Admin.UserName = "simona@tickets.com";
                string adminPWD = "Simona123";
                IdentityResult chkAdmin = await UserManager.CreateAsync(Admin, adminPWD);
                //Add default User to Role Admin
                if (chkAdmin.Succeeded) { var result1 = await UserManager.AddToRoleAsync(Admin, "Admin"); }
            }

            //Add User Role
            var roleCheck1 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck1) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            FlightTicketsUser user = await UserManager.FindByEmailAsync("user@tickets.com");
            if (user == null)
            {
                var User = new FlightTicketsUser();
                User.Email = "user@tickets.com";
                User.UserName = "user@tickets.com";
                string userPWD = "User1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "User"); }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FlightTicketsContext(serviceProvider.GetRequiredService<DbContextOptions<FlightTicketsContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

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

