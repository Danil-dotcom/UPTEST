using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UPTEST.Data;
using UPTEST.Models;

namespace UPTEST.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Customer)
                .Include(o => o.ModifiedBy)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Customer)
                .Include(o => o.ModifiedBy)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            return order == null ? NotFound() : View(order);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateSelectionsAsync();
            return View(new Order
            {
                CreatedAt = DateTime.Now,
                Status = OrderStatuses.Accepted,
                Priority = OrderPriorities.Normal,
                PaymentStatus = PaymentStatuses.Pending
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderNumber,UserId,CustomerId,CustomerName,CustomerPhone,CategoryId,ItemDescription,StainType,Quantity,Status,Priority,TotalPrice,DiscountAmount,FinalPrice,PaymentStatus,PaymentMethod,CreatedAt,PickupDate,CompletedAt,Notes,LastModifiedBy,LastModifiedAt")] Order order)
        {
            NormalizeOrder(order, isNewOrder: true);

            if (await HasDuplicateOrderNumberAsync(order.OrderNumber))
            {
                ModelState.AddModelError(nameof(Order.OrderNumber), "Заказ с таким номером уже существует.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectionsAsync(order);
                return View(order);
            }

            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await PopulateSelectionsAsync(order);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderNumber,UserId,CustomerId,CustomerName,CustomerPhone,CategoryId,ItemDescription,StainType,Quantity,Status,Priority,TotalPrice,DiscountAmount,FinalPrice,PaymentStatus,PaymentMethod,CreatedAt,PickupDate,CompletedAt,Notes,LastModifiedBy,LastModifiedAt")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            NormalizeOrder(order, isNewOrder: false);

            if (await HasDuplicateOrderNumberAsync(order.OrderNumber, order.OrderId))
            {
                ModelState.AddModelError(nameof(Order.OrderNumber), "Заказ с таким номером уже существует.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectionsAsync(order);
                return View(order);
            }

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Customer)
                .Include(o => o.ModifiedBy)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            return order == null ? NotFound() : View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelectionsAsync(Order? order = null)
        {
            ViewData["CategoryId"] = new SelectList(
                await _context.ItemCategories.AsNoTracking().OrderBy(c => c.CategoryName).ToListAsync(),
                "CategoryId",
                "CategoryName",
                order?.CategoryId);

            ViewData["CustomerId"] = new SelectList(
                await _context.Customers.AsNoTracking().OrderBy(c => c.FullName).ToListAsync(),
                "CustomerId",
                "FullName",
                order?.CustomerId);

            ViewData["LastModifiedBy"] = new SelectList(
                await _context.Users.AsNoTracking().OrderBy(u => u.FullName).ToListAsync(),
                "UserId",
                "FullName",
                order?.LastModifiedBy);

            ViewData["UserId"] = new SelectList(
                await _context.Users.AsNoTracking().OrderBy(u => u.FullName).ToListAsync(),
                "UserId",
                "FullName",
                order?.UserId);

            ViewData["StatusOptions"] = new SelectList(OrderStatuses.All, order?.Status);
            ViewData["PriorityOptions"] = new SelectList(OrderPriorities.All, order?.Priority);
            ViewData["PaymentStatusOptions"] = new SelectList(PaymentStatuses.All, order?.PaymentStatus);
        }

        private void NormalizeOrder(Order order, bool isNewOrder)
        {
            order.OrderNumber = order.OrderNumber?.Trim();
            order.CustomerName = order.CustomerName?.Trim();
            order.CustomerPhone = order.CustomerPhone?.Trim();
            order.ItemDescription = order.ItemDescription?.Trim();
            order.StainType = order.StainType?.Trim();
            order.PaymentMethod = string.IsNullOrWhiteSpace(order.PaymentMethod) ? null : order.PaymentMethod.Trim();
            order.Notes = string.IsNullOrWhiteSpace(order.Notes) ? null : order.Notes.Trim();

            if (string.IsNullOrWhiteSpace(order.Status) || !OrderStatuses.IsValid(order.Status))
            {
                ModelState.AddModelError(nameof(Order.Status), "Выберите корректный статус заказа.");
            }

            if (!OrderPriorities.All.Contains(order.Priority))
            {
                ModelState.AddModelError(nameof(Order.Priority), "Выберите корректный приоритет заказа.");
            }

            if (!PaymentStatuses.All.Contains(order.PaymentStatus))
            {
                ModelState.AddModelError(nameof(Order.PaymentStatus), "Выберите корректный статус оплаты.");
            }

            if (order.DiscountAmount < 0)
            {
                ModelState.AddModelError(nameof(Order.DiscountAmount), "Скидка не может быть отрицательной.");
            }

            if (order.FinalPrice < 0 || order.TotalPrice < 0)
            {
                ModelState.AddModelError(nameof(Order.FinalPrice), "Сумма заказа должна быть неотрицательной.");
            }

            if (order.FinalPrice > order.TotalPrice && order.DiscountAmount > 0)
            {
                ModelState.AddModelError(nameof(Order.FinalPrice), "Итоговая сумма не может превышать общую при наличии скидки.");
            }

            if (order.PickupDate.HasValue && order.PickupDate < order.CreatedAt)
            {
                ModelState.AddModelError(nameof(Order.PickupDate), "Дата выдачи не может быть раньше даты создания.");
            }

            if (order.CompletedAt.HasValue && order.CompletedAt < order.CreatedAt)
            {
                ModelState.AddModelError(nameof(Order.CompletedAt), "Дата завершения не может быть раньше даты создания.");
            }

            if (isNewOrder && order.CreatedAt == default)
            {
                order.CreatedAt = DateTime.Now;
            }

            order.LastModifiedAt = DateTime.Now;
        }

        private Task<bool> HasDuplicateOrderNumberAsync(string? orderNumber, int? currentOrderId = null)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return Task.FromResult(false);
            }

            return _context.Orders.AnyAsync(o =>
                o.OrderNumber == orderNumber &&
                (!currentOrderId.HasValue || o.OrderId != currentOrderId.Value));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
