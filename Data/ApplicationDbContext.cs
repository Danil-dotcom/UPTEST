using Microsoft.EntityFrameworkCore;

namespace UPTEST.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UPTEST.Models.Role> Roles { get; set; }
        public DbSet<UPTEST.Models.User> Users { get; set; }
        public DbSet<UPTEST.Models.Customer> Customers { get; set; }
        public DbSet<UPTEST.Models.ItemCategory> ItemCategories { get; set; }
        public DbSet<UPTEST.Models.Service> Services { get; set; }
        public DbSet<UPTEST.Models.Order> Orders { get; set; }
        public DbSet<UPTEST.Models.OrderDetail> OrderDetails { get; set; }
        public DbSet<UPTEST.Models.OrderHistory> OrderHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Уникальные индексы
            modelBuilder.Entity<UPTEST.Models.Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<UPTEST.Models.User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UPTEST.Models.Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<UPTEST.Models.ItemCategory>()
                .HasIndex(ic => ic.CategoryName)
                .IsUnique();

            modelBuilder.Entity<UPTEST.Models.Service>()
                .HasIndex(s => s.ServiceName)
                .IsUnique();

            modelBuilder.Entity<UPTEST.Models.Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            // ========== НАСТРОЙКА ВСЕХ СВЯЗЕЙ ==========

            // Role -> Users
            modelBuilder.Entity<UPTEST.Models.Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Orders (кто создал заказ)
            modelBuilder.Entity<UPTEST.Models.User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> OrderHistory (кто изменил)
            modelBuilder.Entity<UPTEST.Models.User>()
                .HasMany(u => u.OrderHistories)
                .WithOne(oh => oh.User)
                .HasForeignKey(oh => oh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer -> Orders
            modelBuilder.Entity<UPTEST.Models.Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // ItemCategory -> Orders
            modelBuilder.Entity<UPTEST.Models.ItemCategory>()
                .HasMany(ic => ic.Orders)
                .WithOne(o => o.Category)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service -> OrderDetails
            modelBuilder.Entity<UPTEST.Models.Service>()
                .HasMany(s => s.OrderDetails)
                .WithOne(od => od.Service)
                .HasForeignKey(od => od.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderDetails
            modelBuilder.Entity<UPTEST.Models.Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Только здесь Cascade

            // Order -> OrderHistory
            modelBuilder.Entity<UPTEST.Models.Order>()
                .HasMany(o => o.OrderHistories)
                .WithOne(oh => oh.Order)
                .HasForeignKey(oh => oh.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Только здесь Cascade

            // Order -> ModifiedBy (кто последний изменил)
            modelBuilder.Entity<UPTEST.Models.Order>()
                .HasOne(o => o.ModifiedBy)
                .WithMany()
                .HasForeignKey(o => o.LastModifiedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Точность decimal
            modelBuilder.Entity<UPTEST.Models.Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.Order>()
                .Property(o => o.DiscountAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.Order>()
                .Property(o => o.FinalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.OrderDetail>()
                .Property(od => od.PricePerUnit)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.OrderDetail>()
                .Property(od => od.Subtotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.Service>()
                .Property(s => s.BasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UPTEST.Models.Customer>()
                .Property(c => c.TotalSpent)
                .HasPrecision(10, 2);
        }
    }
}