using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBillingMVC.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;

namespace HotelBillingMVC.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly Hotel_Billing_SystemContext _context;

        public ReservationsController(Hotel_Billing_SystemContext context )
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(string searchBy, string search)
        {
            if (searchBy == "ReservationStatus")
            {
                return View(_context.Reservations.Include(r => r.ReservationBranches).Include(r => r.ReservationTables).Include(r => r.ReservationTimeslots).Include(r => r.Reservationuser).Include(r => r.ResevationRest).Where(x => x.ReservationStatus == search || search == null).ToList());
            }
            else
            {
                return View(_context.Reservations.Include(r => r.ReservationBranches).Include(r => r.ReservationTables).Include(r => r.ReservationTimeslots).Include(r => r.Reservationuser).Include(r => r.ResevationRest).Where(x => x.Reservationuser.Username.StartsWith(search) || search == null).ToList());
            }
            var hotel_Billing_SystemContext = _context.Reservations.Include(r => r.ReservationBranches).Include(r => r.ReservationTables).Include(r => r.ReservationTimeslots).Include(r => r.Reservationuser).Include(r => r.ResevationRest);
            return View(await hotel_Billing_SystemContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.ReservationBranches)
                .Include(r => r.ReservationTables)
                .Include(r => r.ReservationTimeslots)
                .Include(r => r.Reservationuser)
                .Include(r => r.ResevationRest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        

        [HttpGet]
        public async Task<IActionResult> GetSeatNumberByCapacity(string capacity)
        {
            var diningTable = await _context.DiningTables
                                             .Where(dt => dt.Capacity == capacity)
                                            .FirstOrDefaultAsync();
            //return Json(diningTable);

            if (diningTable != null)
            {
                return Json(new { seatNo = diningTable.SeatNo });
            }

            return Json(new { seatNo = "Not Available" });
        }


        // GET: Reservations/Create
        public IActionResult Create()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            var user = JsonConvert.DeserializeObject<Usermaster>(userSession);

            var model = new Reservation
            {
                ReservationuserId = user.Id,
                ReservationStatus = "Booked"
            };

            

            ViewData["ReservationBranchName"] = new SelectList(_context.Branches, "Id", "BranchName");
            //ViewBag.ReservationTables = new SelectList(_context.DiningTables.Select(t => t.Capacity).Distinct().ToList());

            ViewData["ReservationTablesId"] = new SelectList(_context.DiningTables, "Id", "Capacity");
            ViewData["ReservationTimeslotsId"] = new SelectList(_context.Timeslots, "Id", "MealType");
            ViewData["ReservationuserId"] = new SelectList(_context.Usermasters, "Id", "Username");           
            ViewData["ResevationRestId"] = new SelectList(_context.Restaurents, "Id", "RestaurentName");
                      
            return View();
        }



        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {

            if (ModelState.IsValid)
            {
                
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                await SendBookingConfirmationEmail(reservation);

                TempData["SuccessMessage"] = "Table Booking successfully!...\nYour Booking Details sending your Register Email Address...Please Check it...";
                //TempData["SuccessMessage"] = $"Table Booking successfully. Your Seat Number is 2 {reservation.ReservationTables.SeatNo}.";
                return RedirectToAction(nameof(Create));
            }
                ViewData["ReservationBranchName"] = new SelectList(_context.Branches, "Id", "BranchName", reservation.ReservationBranchesId);
                ViewData["ReservationTablesId"] = new SelectList(_context.DiningTables, "Id", "Capacity", reservation.ReservationTablesId);
              //  ViewBag.ReservationTables = new SelectList(_context.DiningTables.Select(t => t.Capacity).Distinct().ToList());
                ViewData["ReservationTimeslotsId"] = new SelectList(_context.Timeslots, "Id", "MealType", reservation.ReservationTimeslotsId);
                ViewData["ReservationuserId"] = new SelectList(_context.Usermasters, "Id", "Username", reservation.ReservationuserId);
                ViewData["ResevationRestId"] = new SelectList(_context.Restaurents, "Id", "RestaurentName", reservation.ResevationRestId);

                return View(reservation);
            

        }
        public async Task<IActionResult> UpdateResStatus(int Id, string status)
        {
            var res = await _context.Reservations.FindAsync(Id);
            if (res == null)
            {
                return NotFound();
            }

            res.ReservationStatus = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private async Task SendBookingConfirmationEmail(Reservation reservation)
        {
            try
            {
                var user = await _context.Usermasters.FindAsync(reservation.ReservationuserId);
                var diningTable = await _context.DiningTables.FindAsync(reservation.ReservationTablesId);
                var branch = await _context.Branches.FindAsync(reservation.ReservationBranchesId);

                if (user == null || diningTable == null || branch == null)
                {
                    throw new Exception("Failed to retrieve user, dining table, or branch information.");
                }

                // Configure SMTP client
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("niknisvis123@gmail.com", "gzog xjhu qdyp lszf");
                    smtpClient.EnableSsl = true;

                    // Construct email message
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("niknisvis123@gmail.com", "Fenvial Hotel");
                        mailMessage.To.Add(user.Email); // Replace with recipient's email
                        mailMessage.Subject = "Table Booking Confirmation";
                        mailMessage.Body = $"Dear {user.Username},\nFenvial Hotel \n\nYour table booking at {branch.BranchName} has been confirmed.\n\nDining Number: {diningTable.SeatNo}\nReservation Date: {reservation.ReservationDate}\nCapacity: {diningTable.Capacity}\n\nThank you for choosing our restaurant!";
                        mailMessage.IsBodyHtml = false;

                        // Send email
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception as per your application's error handling strategy
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }
        // GET: Reservations/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ReservationBranchesId"] = new SelectList(_context.Branches, "Id", "Id", reservation.ReservationBranchesId);
            ViewData["ReservationTablesId"] = new SelectList(_context.DiningTables, "Id", "Id", reservation.ReservationTablesId);
            ViewData["ReservationTimeslotsId"] = new SelectList(_context.Timeslots, "Id", "Id", reservation.ReservationTimeslotsId);
            ViewData["ReservationuserId"] = new SelectList(_context.Usermasters, "Id", "Id", reservation.ReservationuserId);
            ViewData["ResevationRestId"] = new SelectList(_context.Restaurents, "Id", "Id", reservation.ResevationRestId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ReservationuserId,TableId,ReservationTablesId,TimeslteId,ReservationTimeslotsId,BranchId,ReservationBranchesId,RestaurentId,ResevationRestId,ReservationDate,ReservationStatus,ActiveFlag")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["ReservationBranchesId"] = new SelectList(_context.Branches, "Id", "Id", reservation.ReservationBranchesId);
            ViewData["ReservationTablesId"] = new SelectList(_context.DiningTables, "Id", "Id", reservation.ReservationTablesId);
            ViewData["ReservationTimeslotsId"] = new SelectList(_context.Timeslots, "Id", "Id", reservation.ReservationTimeslotsId);
            ViewData["ReservationuserId"] = new SelectList(_context.Usermasters, "Id", "Id", reservation.ReservationuserId);
            ViewData["ResevationRestId"] = new SelectList(_context.Restaurents, "Id", "Id", reservation.ResevationRestId);

            return View(reservation);
        }*/
        /* public IActionResult Cancel(int id)
         {
             var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

             if (reservation == null)
             {
                 return NotFound();
             }

             return View(reservation);
         }

         // POST: Reservation/Cancel/{id}
         [HttpPost, ActionName("Cancel")]
         [ValidateAntiForgeryToken]
         public IActionResult CancelConfirmed(int id)
         {
             var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

             if (reservation == null)
             {
                 return NotFound();
             }

             // Perform cancellation process here (e.g., update reservation status)
             reservation.ReservationStatus = "Cancelled";
             _context.SaveChanges();

             // Redirect to a confirmation page or show a success message
             return RedirectToAction("Create", "Reservations");
         }

         // GET: Reservations/Delete/5
         public async Task<IActionResult> Delete(int? id)
         {
             if (id == null || _context.Reservations == null)
             {
                 return NotFound();
             }

             var reservation = await _context.Reservations
                 .Include(r => r.ReservationBranches)
                 .Include(r => r.ReservationTables)
                 .Include(r => r.ReservationTimeslots)
                 .Include(r => r.Reservationuser)
                 .Include(r => r.ResevationRest)
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (reservation == null)
             {
                 return NotFound();
             }

             return View(reservation);
         }

         // POST: Reservations/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             if (_context.Reservations == null)
             {
                 return Problem("Entity set 'Hotel_Billing_SystemContext.Reservations'  is null.");
             }
             var reservation = await _context.Reservations.FindAsync(id);
             if (reservation != null)
             {
                 _context.Reservations.Remove(reservation);
             }

             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
         }*/

    }
}
