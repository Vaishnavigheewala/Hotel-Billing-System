using HotelBillingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace HotelBillingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Hotel_Billing_SystemContext context;

        public HomeController(Hotel_Billing_SystemContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult firstpage()
        {
            return View();
        }
        public IActionResult help()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usermaster login)
        {
            if (login.Email == "vaishnavigheewala@gmail.com" && login.Password == "va123")
            {
                HttpContext.Session.SetString("UserSession", "admin");
                HttpContext.Session.SetString("UserRole", "admin");
                return RedirectToAction("AdminDashboard");
            }
            var asd = context.Usermasters.Include(u => u.Role).Where(s => s.Email == login.Email && s.Password == login.Password).FirstOrDefault();

            if (asd != null)
            {
                string user = JsonConvert.SerializeObject(asd);
                HttpContext.Session.SetString("UserSession", user);
                if (asd.Role != null)
                {
                    HttpContext.Session.SetString("UserRole", asd.Role.RoleName); // Assuming Role is a navigation property and RoleName is the role's name

                    
                     if (asd.Role.RoleName == "user")
                    {
                        return RedirectToAction("Dashboard");
                    }
                }
                else
                {
                     return RedirectToAction("Login");
                    //ModelState.AddModelError(string.Empty, "User role is not defined.");

                }
            }
            else
            {
                //ViewBag.Message = "Login Fail";
                ModelState.AddModelError(string.Empty, "Invalid email or password.");

            }
            return View();
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") == "user")
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();

            }
            else 
            { 
                return RedirectToAction("Login"); 
            }
            return View();
        }
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Usermasters.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    var token = GenerateResetToken();
                    HttpContext.Session.SetString("ResetToken", token);
                    HttpContext.Session.SetString("ResetEmail", user.Email);

                    var resetLink = Url.Action("ResetPassword", "Home", new { token = token, email = user.Email }, Request.Scheme);
                    SendResetEmail(user.Email, resetLink);
                }

                ViewBag.Message = "If your email exists in our system, you will receive a password reset link.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var sessionToken = HttpContext.Session.GetString("ResetToken");
            var sessionEmail = HttpContext.Session.GetString("ResetEmail");

            if (sessionToken == token && sessionEmail == email)
            {
                var model = new ResetPasswordViewModel { Token = token, Email = email };
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = HttpContext.Session.GetString("ResetToken");
                var sessionEmail = HttpContext.Session.GetString("ResetEmail");

                if (sessionToken == model.Token && sessionEmail == model.Email)
                {
                    var user = context.Usermasters.SingleOrDefault(u => u.Email == model.Email);
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        context.SaveChanges();

                        HttpContext.Session.Remove("ResetToken");
                        HttpContext.Session.Remove("ResetEmail");

                        TempData["Success"] = "Password has been Changed successfully.";
                        return RedirectToAction();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid token or token expired.");
                }
            }

            return View(model);
        }

        private string GenerateResetToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        private void SendResetEmail(string email, string resetLink)
        {
            using (var message = new MailMessage("niknisvis123@gmail.com", email))
            {
                message.From = new MailAddress("niknisvis123@gmail.com", "Fenvial Hotel");
                message.Subject = "Password Reset";
                message.Body = $"Please reset your password by clicking <a href=\"{resetLink}\">here</a>.";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com", 587)) // Update SMTP server and port
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("niknisvis123@gmail.com", "gzog xjhu qdyp lszf");
                    client.EnableSsl = true;  // Use SSL/TLS for secure communication
                    client.Send(message);


                }
            }
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
