using Microsoft.EntityFrameworkCore;

namespace Payment_Service

{
    public partial class PaymentDBContext : DbContext
    {
        public PaymentDBContext()
        {
            //Database.EnsureCreated();
        }

        public PaymentDBContext(DbContextOptions<PaymentDBContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public virtual DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    $"Host=postgres;Port=5432;Database=payments;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.HasIndex(e => e.PaymentUid, "payment_payment_uid_key")
                .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.PaymentUid).HasColumnName("payment_uid");

                entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
