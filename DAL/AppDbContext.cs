using Microsoft.EntityFrameworkCore;
using Restabook.DAL.Entities;

namespace CodeofAzerbaijan.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Menu>? Menus { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<MenuFood>? MenuFoods { get; set; }
    }
}
