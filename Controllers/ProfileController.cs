using HotelBillingMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelBillingMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Hotel_Billing_SystemContext _userManager;
        private string baseURL = "http://localhost:5296/api/Registration";
        private HttpClient client = new HttpClient();

        public ProfileController(Hotel_Billing_SystemContext userManager)
        {
            _userManager = userManager;

        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Index()
        {
            Usermaster user = JsonConvert.DeserializeObject<Usermaster>(HttpContext.Session.GetString("UserSession"));
            
            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(user);

            }
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (userSession == null)
            {
                return RedirectToAction("Index");
            }

            var user = JsonConvert.DeserializeObject<Usermaster>(userSession);
            var userFromDb = _userManager.Usermasters.Find(user.Id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            var model = new Usermaster
            {
                Id = userFromDb.Id,
                Username = userFromDb.Username,
                Email = userFromDb.Email,
                PhoneNumber = userFromDb.PhoneNumber,
                Gender = userFromDb.Gender
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Usermaster model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            var userFromDb = await _userManager.Usermasters.FindAsync(model.Id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            userFromDb.Username = model.Username;
            userFromDb.Email = model.Email;
            userFromDb.PhoneNumber = model.PhoneNumber;
            userFromDb.Gender = model.Gender;

            _userManager.Update(userFromDb);
            await _userManager.SaveChangesAsync();

            // Update the session with the new user data
            HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(userFromDb));
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(EditProfile));
        }

    }
}
