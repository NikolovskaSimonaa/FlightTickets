using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightTickets.Data;
using FlightTickets.Models;
using FlightTickets.ViewModels;
using System.Security.Principal;
using System.Net.Sockets;
using Microsoft.AspNetCore.Identity;
using FlightTickets.Areas.Identity.Data;

namespace FlightTickets.Controllers
{
    public class PassengersController : Controller
    {
        private readonly FlightTicketsContext _context;
        private readonly UserManager<FlightTicketsUser> _userManager;

        public PassengersController(FlightTicketsContext context, UserManager<FlightTicketsUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        private Task<FlightTicketsUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Passengers
        public async Task<IActionResult> Index(string searchPassenger)
        {
            IQueryable<Passenger> passengers = _context.Passenger.AsQueryable();
            if (!string.IsNullOrEmpty(searchPassenger))
            {
                passengers = passengers.Where(s => s.Name.Contains(searchPassenger));
            }
            passengers = passengers.Include(m => m.MyTickets).ThenInclude(p => p.Flight);
            var passengerVM = new SearchPassengerViewModel
            {
                Passengers = await passengers.ToListAsync()
            };
            return View(passengerVM);
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Passenger == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger.Include(m => m.MyTickets).ThenInclude(m => m.Flight).FirstOrDefaultAsync(m => m.Id == id);

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,PassportIdentificationNumber,AppUser")] Passenger passenger)
        {
            Passenger p = new Passenger();
            p.Id = (int)passenger.Id;
            p.Name = passenger.Name;
            p.DateOfBirth = passenger.DateOfBirth;
            p.PassportIdentificationNumber = passenger.PassportIdentificationNumber;
            var user = await GetCurrentUserAsync();
            p.AppUser = user.UserName;
            _context.Add(p);
            await _context.SaveChangesAsync();
            TempData["SuccessfullRegister"] = true;
            return View(passenger);
        }

        public async Task<IActionResult> UserPassengers()
        {
            var user = await GetCurrentUserAsync();
            var PassengersList = _context.Passenger.AsQueryable();
            var UserPassengers = PassengersList
                    .Where(m => m.AppUser == user.UserName)
                    .ToList();
            if (UserPassengers != null)
            {
                return View("~/Views/Passengers/UserPassengers.cshtml", UserPassengers);
            }
            else
            {
                return Problem("Entity set 'FlightTicketsContext.MyPassengers' is null!");
            }
        }

        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passenger == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,PassportIdentificationNumber,AppUser")] Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return NotFound();
            }

            try
            {
                var user = await GetCurrentUserAsync();
                passenger.AppUser = user.UserName;
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(passenger.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(UserPassengers));
        }

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passenger == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passenger == null)
            {
                return Problem("Entity set 'FlightTicketsContext.Passenger'  is null.");
            }
            var passenger = await _context.Passenger.FindAsync(id);
            if (passenger != null)
            {
                _context.Passenger.Remove(passenger);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
          return (_context.Passenger?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
