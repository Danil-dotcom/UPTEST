using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPTEST.Data;
using UPTEST.Models;

namespace UPTEST.Controllers
{
    [Authorize]
    public class ItemCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.ItemCategories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.ItemCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            return category == null ? NotFound() : View(category);
        }

        public IActionResult Create()
        {
            return View(new ItemCategory
            {
                BasePriceMultiplier = 1.0m,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,BasePriceMultiplier,RequiresSpecialCare,IsActive")] ItemCategory category)
        {
            NormalizeCategory(category);

            if (await HasDuplicateCategoryNameAsync(category.CategoryName))
            {
                ModelState.AddModelError(nameof(ItemCategory.CategoryName), "Категория с таким названием уже существует.");
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.ItemCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,BasePriceMultiplier,RequiresSpecialCare,IsActive")] ItemCategory category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            NormalizeCategory(category);

            if (await HasDuplicateCategoryNameAsync(category.CategoryName, category.CategoryId))
            {
                ModelState.AddModelError(nameof(ItemCategory.CategoryName), "Категория с таким названием уже существует.");
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
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

            var category = await _context.ItemCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            return category == null ? NotFound() : View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.ItemCategories
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (category.Orders?.Any() == true)
            {
                TempData["ErrorMessage"] = "Нельзя удалить категорию, которая используется в заказах.";
                return RedirectToAction(nameof(Index));
            }

            _context.ItemCategories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void NormalizeCategory(ItemCategory category)
        {
            category.CategoryName = category.CategoryName?.Trim();
            category.Description = string.IsNullOrWhiteSpace(category.Description) ? null : category.Description.Trim();
        }

        private async Task<bool> HasDuplicateCategoryNameAsync(string? categoryName, int? currentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return false;
            }

            return await _context.ItemCategories.AnyAsync(c =>
                c.CategoryName != null &&
                c.CategoryName.ToLower() == categoryName.ToLower() &&
                (!currentCategoryId.HasValue || c.CategoryId != currentCategoryId.Value));
        }

        private bool CategoryExists(int id)
        {
            return _context.ItemCategories.Any(e => e.CategoryId == id);
        }
    }
}
