using System.Threading.Tasks;
using Funkmap.Payments.Data.Entities;
using Funkmap.Payments.Data.Tools;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
        {

        }

        public DbSet<PaymentEntity> Payments { get; set; }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductLocaleEntity> ProductLocales { get; set; }
        public DbSet<PayPalPlanEntity> PayPalPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductLocaleEntity>()
                .HasOne<ProductEntity>()
                .WithMany(x => x.ProductLocales)
                .HasForeignKey(x => x.ProductName);

            modelBuilder.Entity<ProductEntity>().HasData(PaymentsStaticDatProdiver.Products);
            modelBuilder.Entity<ProductLocaleEntity>().HasData(PaymentsStaticDatProdiver.ProductLocales);

            modelBuilder.Entity<PayPalPlanEntity>()
                .HasOne<ProductLocaleEntity>()
                .WithOne()
                .HasForeignKey<PayPalPlanEntity>(x => x.ProductLocaleId);

        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}