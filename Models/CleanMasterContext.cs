using Microsoft.EntityFrameworkCore;

namespace UPTEST.Models
{
    /// <summary>
    /// Контекст базы данных для Entity Framework Core
    /// </summary>
    public class CleanMasterContext : DbContext
    {
        public CleanMasterContext(DbContextOptions<CleanMasterContext> options)
            : base(options)
        {
        }

        // DbSet для всех таблиц
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHistory> OrderHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка уникальных индексов
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.DiscountCard)
                .IsUnique();

            modelBuilder.Entity<ItemCategory>()
                .HasIndex(ic => ic.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Service>()
                .HasIndex(s => s.ServiceName)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            // Настройка связей и каскадного удаления
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderHistory>()
                .HasOne(oh => oh.Order)
                .WithMany(o => o.OrderHistories)
                .HasForeignKey(oh => oh.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка точности decimal
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.FinalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.PricePerUnit)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Subtotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.BasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Customer>()
                .Property(c => c.TotalSpent)
                .HasPrecision(10, 2);

            // Начальные данные (Seed Data)
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Администратор системы" },
                new Role { RoleId = 2, RoleName = "Manager", Description = "Менеджер" },
                new Role { RoleId = 3, RoleName = "User", Description = "Обычный пользователь" }
            );

            modelBuilder.Entity<ItemCategory>().HasData(
                new ItemCategory { CategoryId = 1, CategoryName = "Верхняя одежда", Description = "Пальто, куртки, пуховики" },
                new ItemCategory { CategoryId = 2, CategoryName = "Костюмы", Description = "Деловые костюмы, пиджаки" },
                new ItemCategory { CategoryId = 3, CategoryName = "Платья", Description = "Вечерние и повседневные платья" },
                new ItemCategory { CategoryId = 4, CategoryName = "Рубашки и блузы", Description = "Рубашки, блузы, топы" },
                new ItemCategory { CategoryId = 5, CategoryName = "Трикотаж", Description = "Свитера, джемперы" }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { ServiceId = 1, ServiceName = "Химчистка стандартная", BasePrice = 1000.00m, DurationHours = 24 },
                new Service { ServiceId = 2, ServiceName = "Химчистка интенсивная", BasePrice = 1500.00m, DurationHours = 48 },
                new Service { ServiceId = 3, ServiceName = "Выведение пятен", BasePrice = 500.00m, DurationHours = 6 },
                new Service { ServiceId = 4, ServiceName = "Глажка", BasePrice = 300.00m, DurationHours = 4 }
            );
        }
    }
}