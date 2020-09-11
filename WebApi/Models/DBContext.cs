using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Odt;
using WebApi.Services;

namespace WebApi
{
    public class DBContext : DbContext
    {
        public DbSet<Factory> Factory { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Tank> Tank { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<EventOdt> Event { get; set; }

        public DBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var md5 = new MD5Hash("pwd123");
            var hash = md5.GetMd5Hash();
            modelBuilder.Entity<Person>().HasData(new Models.Person {Id =1, Login = "admin", hashPassword = hash, Role = "admin"});
        }
    }
}
