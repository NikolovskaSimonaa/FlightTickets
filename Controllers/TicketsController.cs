using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightTickets.Data;
using FlightTickets.Models;
using Microsoft.AspNetCore.Identity;
using FlightTickets.Areas.Identity.Data;

namespace FlightTickets.Controllers
{
    public class TicketsController : Controller
    {
        private readonly FlightTicketsContext _context;
        private readonly UserManager<FlightTicketsUser> _userManager;

        public TicketsController(FlightTicketsContext context, UserManager<FlightTicketsUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }
        private Task<FlightTicketsUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Ticket
            .Include(m => m.Passenger)
            .Include(m => m.Flight).ThenInclude(m => m.Tickets)
            .ToListAsync();
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.Include(m => m.Passenger).Include(m => m.Flight).FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.PassengerId = new SelectList(_context.Passenger, "Id", "Id");
            ViewBag.FlightId = new SelectList(_context.Flight, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PassengerId,FlightId,SeatNumber,AppUser")] Ticket ticket)
        {

            bool isSeatAvaliable = !_context.Ticket.Any(t => t.FlightId == ticket.FlightId && t.SeatNumber == ticket.SeatNumber);

            if ( isSeatAvaliable)
            {
                Ticket t = new Ticket();
                t.Id = (int)ticket.Id;
                t.PassengerId = (int)ticket.PassengerId;
                t.FlightId = (int)ticket.FlightId;
                t.SeatNumber = (int)ticket.SeatNumber;
                var user = await GetCurrentUserAsync();
                t.AppUser = user.UserName;
                _context.Add(t);
                await _context.SaveChangesAsync();
                TempData["IsBookingSuccessful"] = true;
            }
            else if (!isSeatAvaliable)
            { //if a ticket with the same seat number for that flight is already taken
                ModelState.AddModelError("SeatNumber", "This seat is already taken for this flight. Try another one, or contact us for help.");
                TempData["IsBookingSuccessful"] = false;
            }
            else {
                TempData["IsBookingSuccessful"] = false;
            }
            return View(ticket);
        }

        public async Task<IActionResult> UserTickets()
        {
            var user = await GetCurrentUserAsync();
            var MyTicketsList = _context.Ticket.AsQueryable();
            var UserTickets = MyTicketsList
                    .Include(m => m.Passenger)
                    .Include(m => m.Flight)
                    .ThenInclude(m => m.Tickets)
                    .Where(m => m.AppUser == user.UserName)
                    .ToList();
            if (UserTickets != null)
            {
                return View("~/Views/Tickets/UserTickets.cshtml", UserTickets);
            }
            else
            {
                return Problem("Entity set 'FlightTicketsContext.MyTickets' is null!");
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PassengerId,FlightId,SeatNumber")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'FlightTicketsContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
