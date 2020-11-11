using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataBase
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

            modelBuilder.Entity<AdDb>()
                .Property(p => p.Number)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AdDb>()
                .Property(p => p.Number)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<RatingDb>()
                .HasOne(p => p.User)
                .WithMany(t => t.Ratings)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RatingDb>()
                .HasOne(p => p.Ad)
                .WithMany(t => t.Ratings)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}