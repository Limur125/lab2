
using Microsoft.EntityFrameworkCore;

namespace Loyalty_Service
{
    public partial class LoyaltyDBContext(DbContextOptions<LoyaltyDBContext> options) : DbContext(options)
    {
        public virtual DbSet<Loyalty> Loyalty { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=postgres;Port=5432;Database=loyalties;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Loyalty>(entity =>
            {
                entity.ToTable("loyalty");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Username)
                        .HasMaxLength(80)
                        .HasColumnName("username");

                entity.Property(e => e.ReservationCount).HasColumnName("reservation_count");

                entity.Property(e => e.Status)
                .HasMaxLength(80)
                .HasColumnName("status")
                .HasDefaultValueSql("'BRONZE'::character varying");

                entity.Property(e => e.Discount).HasColumnName("discount").HasDefaultValueSql("5::character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
