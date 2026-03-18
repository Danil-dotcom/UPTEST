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
    }
}
