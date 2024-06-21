using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBillingMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Data;


namespace HotelBillingMVC.Controllers
{
    public class UsermastersController : Controller
    {
        private readonly Hotel_Billing_SystemContext _context;

        private string baseURL = "http://localhost:5296/api/Registration";
        private HttpClient client = new HttpClient();


        public UsermastersController(Hotel_Billing_SystemContext context)
        {
            _context = context;
        }

        // GET: Usermasters
        public async Task<IActionResult> Index()
        {

            var hotel_Billing_SystemContext = _context.Usermasters.Include(u => u.Role);
            return View(await hotel_Billing_SystemContext.ToListAsync());
        }

        // GET: Usermasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usermasters == null)
            {
                return NotFound();
            }

            var usermaster = await _context.Usermasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usermaster == null)
            {
                return NotFound();
            }

            return View(usermaster);
        }

        

        // GET: Usermasters/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Usermasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,PhoneNumber,Gender,RoleId,ActiveFlag")] Usermaster usermaster)
        {
            //if (ModelState.IsValid)
            //{
                string defaultRoleName = "User"; // Set your default role name here
                var role = await _context.RoleMasters.FirstOrDefaultAsync(r => r.RoleName == defaultRoleName);

                if (role != null)
                {
                    usermaster.RoleId = role.Id;

                    _context.Add(usermaster);
                    //await _context.SaveChangesAsync();
                    //TempData["SuccessMessage"] = "Registration successful. You can now log in.";

                    //return RedirectToAction("Login", "Home");
                    try
                    {
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Registration successful. You can now log in.";
                        return RedirectToAction("Login", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, Please fill up all required fields.");
                        // Log the error (uncomment ex variable name and write a log.)
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Default role not found. Please contact support.");
                }
            //}
               
            //ViewBag.RoleId = new SelectList(_context.RoleMasters, "Id", "RoleName", usermaster.RoleId);
            return View(usermaster);
     }
        

        // GET: Usermasters/Edit/5
       /* public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usermasters == null)
            {
                return NotFound();
            }

            var usermaster = await _context.Usermasters.FindAsync(id);
            if (usermaster == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.RoleMasters, "Id", "Id", usermaster.RoleId);
            return View(usermaster);
        }

        // POST: Usermasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,PhoneNumber,Gender,RoleId,ActiveFlag")] Usermaster usermaster)
        {
            if (id != usermaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usermaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsermasterExists(usermaster.Id))
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
            ViewData["RoleId"] = new SelectList(_context.RoleMasters, "Id", "Id", usermaster.RoleId);
            return View(usermaster);
        }*/

        // GET: Usermasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usermasters == null)
            {
                return NotFound();
            }

            var usermaster = await _context.Usermasters
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usermaster == null)
            {
                return NotFound();
            }

            return View(usermaster);
        }

        // POST: Usermasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usermasters == null)
            {
                return Problem("Entity set 'Hotel_Billing_SystemContext.Usermasters'  is null.");
            }
            var usermaster = await _context.Usermasters.FindAsync(id);
            if (usermaster != null)
            {
                _context.Usermasters.Remove(usermaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool UsermasterExists(int id)
        {
          return (_context.Usermasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
