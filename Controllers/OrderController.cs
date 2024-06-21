using HotelBillingMVC.Helpers;
using HotelBillingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

namespace HotelBillingMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly Hotel_Billing_SystemContext _context;

        private string baseURL = "http://localhost:5296/api/Menus";
        private HttpClient client = new HttpClient();

        public OrderController(Hotel_Billing_SystemContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchBy , string search)
        {
            if (searchBy == "OrderStatus")
            {
                return View(_context.OrderMasters.Include(o => o.OrderDetails).ThenInclude(od => od.Menu).Where(x => x.OrderStatus == search || search == null).ToList());
            }
            else
            {
                return View(_context.OrderMasters.Include(o => o.OrderDetails).ThenInclude(od => od.Menu).Where(x => x.CustName.StartsWith(search) || search == null).ToList());
            }
            var orders = _context.OrderMasters.Include(o => o.OrderDetails).ThenInclude(od => od.Menu).ToList();
            return View(orders);

            //return View();
        }
        public IActionResult Menu()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderMasters == null)
            {
                return NotFound();
            }

            var orderMaster = await _context.OrderMasters
                .Include(o => o.OrderTables).Include(od => od.OrderDetails).ThenInclude(bi => bi.Menu) // Ensure this includes the Menu navigation property

                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderMaster == null)
            {
                return NotFound();
            }

            return View(orderMaster);
        }

        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "ItemName");
            ViewData["TableId"] = new SelectList(_context.DiningTables, "Id", "SeatNo");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new OrderMaster
                {
                    UserId = model.UserId,
                    TableId = model.TableId,
                    CustName = model.CustName,
                    OrderDate = DateTime.Now,
                    TotalAmount = model.OrderDetails.Sum(od => od.Quantity * _context.Menus.Find(od.MenuId).Price),
                    PaymentType = "Cash",
                    OrderStatus = "Pending",
                    ActiveFlag = true,
                    OrderDetails = model.OrderDetails.Select(od => new OrderDetail
                    {
                        MenuId = od.MenuId,
                        Quantity = od.Quantity,
                        UnitPrice = _context.Menus.Find(od.MenuId).Price,
                        ActiveFlag = true
                    }).ToList()
                };

                _context.OrderMasters.Add(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "ItemName", model.OrderDetails.FirstOrDefault()?.MenuId);
            ViewData["TableId"] = new SelectList(_context.DiningTables, "Id", "SeatNo");

            return View(model);
        }

        public async Task<IActionResult> UpdateStatus(int Id, string status)
        {
            var order = await _context.OrderMasters.FindAsync(Id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Generatepdf(int id)
        {
            var order = await _context.OrderMasters
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Menu)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.OrderStatus == "Pending")
            {
                TempData["ErrorMessage"] = "Cannot generate bill. The Bill status is not 'Complete'.";
                return RedirectToAction("Details", new { id = id });
            }
            else
            {

                //html = html.Replace("StrTag", "<").Replace("EndTag", ">");
                //HtmlToPdf oHtmlToPdf = new HtmlToPdf();
                //PdfDocument opdfDocument = oHtmlToPdf.ConvertHtmlString(html);
                //byte[] pdf = opdfDocument.Save();
                //opdfDocument.Close();

                var pdfBytes = PdfGenerator.GenerateBill(order);

                return File(
                    pdfBytes,
                    "application/pdf",
                    $"Order_{id}_Bill.pdf"
                );
            }
           
        }

    }
}
