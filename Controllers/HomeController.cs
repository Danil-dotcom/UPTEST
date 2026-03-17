using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPTEST.Data;

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
            // Статистика для главной страницы
            ViewBag.OrdersCount = await _context.Orders.CountAsync();
            ViewBag.ActiveOrdersCount = await _context.Orders
                .Where(o => o.Status == "В чистке" || o.Status == "Принят")
                .CountAsync();
            ViewBag.ReadyOrdersCount = await _context.Orders
                .Where(o => o.Status == "Готов")
                .CountAsync();
            ViewBag.CustomersCount = await _context.Customers.CountAsync();

            return View();
        }
    }
}