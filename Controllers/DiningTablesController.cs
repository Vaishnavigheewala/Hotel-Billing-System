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
    public class DiningTablesController : Controller
    {
        private readonly Hotel_Billing_SystemContext _context;

        public DiningTablesController(Hotel_Billing_SystemContext context)
        {
            _context = context;
        }

        // GET: DiningTables
        public async Task<IActionResult> Index()
        {
            var hotel_Billing_SystemContext = _context.DiningTables.Include(d => d.Tables);
            return View(await hotel_Billing_SystemContext.ToListAsync());
        }

        // GET: DiningTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiningTables == null)
            {
                return NotFound();
            }

            var diningTable = await _context.DiningTables
                .Include(d => d.Tables)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diningTable == null)
            {
                return NotFound();
            }

            return View(diningTable);
        }

        // GET: DiningTables/Create
        public IActionResult Create()
        {
            ViewData["TablesId"] = new SelectList(_context.Branches, "Id", "BranchName");
            return View();
        }

        // POST: DiningTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiningTable diningTable)
        {
            if (ModelState.IsValid)
            {
                var dinig = new DiningTable
                {
                    BranchId = 1,
                    TablesId = diningTable.TablesId,
                    SeatNo = diningTable.SeatNo,
                    Capacity = diningTable.Capacity,
                    ActiveFlag = true,

                };
                
                _context.Add(diningTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TablesId"] = new SelectList(_context.Branches, "Id", "Id", diningTable.TablesId);
            return View(diningTable);
        }

        // GET: DiningTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiningTables == null)
            {
                return NotFound();
            }

            var diningTable = await _context.DiningTables.FindAsync(id);
            if (diningTable == null)
            {
                return NotFound();
            }
            ViewData["TablesId"] = new SelectList(_context.Branches, "Id", "BranchName", diningTable.TablesId);
            return View(diningTable);
        }

        // POST: DiningTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BranchId,TablesId,SeatNo,Capacity,ActiveFlag")] DiningTable diningTable)
        {
            if (id != diningTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diningTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiningTableExists(diningTable.Id))
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
            ViewData["TablesId"] = new SelectList(_context.Branches, "Id", "Id", diningTable.TablesId);
            return View(diningTable);
        }

        // GET: DiningTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiningTables == null)
            {
                return NotFound();
            }

            var diningTable = await _context.DiningTables
                .Include(d => d.Tables)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diningTable == null)
            {
                return NotFound();
            }

            return View(diningTable);
        }

        // POST: DiningTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiningTables == null)
            {
                return Problem("Entity set 'Hotel_Billing_SystemContext.DiningTables'  is null.");
            }
            var diningTable = await _context.DiningTables.FindAsync(id);
            if (diningTable != null)
            {
                _context.DiningTables.Remove(diningTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiningTableExists(int id)
        {
          return _context.DiningTables.Any(e => e.Id == id);
        }
    }
}
