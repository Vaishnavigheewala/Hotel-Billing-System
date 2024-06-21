using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBillingMVC.Models;

namespace HotelBillingMVC.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly Hotel_Billing_SystemContext _context;

        public FeedbacksController(Hotel_Billing_SystemContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var hotel_Billing_SystemContext = _context.Feedbacks.Include(f => f.FeedId);
            return View(await hotel_Billing_SystemContext.ToListAsync());
        }
        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            return View(feedback);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.FeedId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'Hotel_Billing_SystemContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
          return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}
