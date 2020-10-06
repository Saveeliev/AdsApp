using Microsoft.EntityFrameworkCore;

namespace AdsApp.Models
{
    public class AdsAppContext : DbContext
    {
        public DbSet<AdDb> Ads { get; set; }
        public DbSet<UserDb> Users { get; set; }
        public DbSet<RatingDb> Ratings { get; set; }

        public AdsAppContext(DbContextOptions<AdsAppContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingDb>()
                .HasIndex(u => new { u.UserId, u.AdId })
                .IsUnique();
        }
    }
}