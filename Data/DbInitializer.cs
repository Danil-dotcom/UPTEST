using Microsoft.EntityFrameworkCore;
using UPTEST.Models;

namespace UPTEST.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { RoleName = "Администратор", Description = "Полный доступ к системе" },
                new Role { RoleName = "Менеджер", Description = "Работа с заказами и клиентами" });

            await context.SaveChangesAsync();
        }

        if (!await context.ItemCategories.AnyAsync())
        {
            context.ItemCategories.AddRange(
                new ItemCategory
                {
                    CategoryName = "Одежда",
                    Description = "Повседневная и деловая одежда",
                    BasePriceMultiplier = 1.0m,
                    RequiresSpecialCare = false,
                    IsActive = true
                },
                new ItemCategory
                {
                    CategoryName = "Верхняя одежда",
                    Description = "Пальто, куртки, пуховики",
                    BasePriceMultiplier = 1.4m,
                    RequiresSpecialCare = true,
                    IsActive = true
                },
                new ItemCategory
                {
                    CategoryName = "Домашний текстиль",
                    Description = "Шторы, пледы, покрывала",
                    BasePriceMultiplier = 1.2m,
                    RequiresSpecialCare = false,
                    IsActive = true
                });

            await context.SaveChangesAsync();
        }
    }
}
