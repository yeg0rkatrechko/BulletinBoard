using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dal
{
    public class BulletinBoardDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BulletinBoardDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Advert> Adverts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AdvertImage> AdvertImages { get; set; } = null!;
        public DbSet<AdvertReaction> AdvertReactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("BulletinBoard"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BulletinBoardDbContext).Assembly);
        }
    }
}
