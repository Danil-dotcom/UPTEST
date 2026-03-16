using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using UPTEST.Models;

namespace UPTEST.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
