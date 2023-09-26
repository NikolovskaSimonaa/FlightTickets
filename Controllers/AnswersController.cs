using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightTickets.Data;
using FlightTickets.Models;
using System.Net.Sockets;
using FlightTickets.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace FlightTickets.Controllers
{
    public class AnswersController : Controller
    {
        private readonly FlightTicketsContext _context;
        private readonly UserManager<FlightTicketsUser> _userManager;

        public AnswersController(FlightTicketsContext context, UserManager<FlightTicketsUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        private Task<FlightTicketsUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var flightTicketsContext = _context.Answer.Include(a => a.Question);
            return View(await flightTicketsContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }
            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int questionId, [Bind("Id,Comment,AppUser,QuestionId")] Answer answer)
        {
            Answer a = new Answer();
            a.Comment = answer.Comment;
            a.QuestionId = questionId;
            var user = await GetCurrentUserAsync();
            a.AppUser = user.UserName;
            _context.Add(a);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,AppUser,QuestionId")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id", answer.QuestionId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Answer == null)
            {
                return Problem("Entity set 'FlightTicketsContext.Answer'  is null.");
            }
            var answer = await _context.Answer.FindAsync(id);
            if (answer != null)
            {
                _context.Answer.Remove(answer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
          return (_context.Answer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
