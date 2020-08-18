using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public class DBContext : DbContext
    {
        public DbSet<Factory> Factory { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Tank> Tank { get; set; }

        public DBContext(DbContextOptions options) : base(options)
        {
            
        }

    }
}
