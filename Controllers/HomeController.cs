using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPTEST.Data;
using UPTEST.Models;

namespace UPTEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.OrdersCount = await _context.Orders.CountAsync();
            ViewBag.ActiveOrdersCount = await _context.Orders
                .Where(o => o.Status == OrderStatuses.Accepted || o.Status == OrderStatuses.InProgress)
                .CountAsync();
            ViewBag.ReadyOrdersCount = await _context.Orders
                .Where(o => o.Status == OrderStatuses.Ready)
                .CountAsync();
            ViewBag.CustomersCount = await _context.Customers.CountAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
