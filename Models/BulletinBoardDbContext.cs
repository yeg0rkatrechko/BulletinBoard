using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.DbModels;

namespace Models
{
    public class BulletinBoardDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BulletinBoardDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Advert> Adverts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdvertImage> AdvertImages { get; set; }
        public DbSet<AdvertReaction> AdvertReactions { get; set; }
        public BulletinBoardDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("BulletinBoard"));
        }
    }
}
