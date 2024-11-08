using Microsoft.EntityFrameworkCore;

namespace AATTechnical
{
    public class Number
    {
        public int Value { get; set; }
        public bool IsPrime { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Number> Numbers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

}
