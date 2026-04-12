using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryControl.Data.Models;

namespace InventoryControl.Data
{
    // Kontekst bazy dannyh dlya raboty cherez EF Core i Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Konstruktor konteksta
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablica tovarov
        public DbSet<Product> Products { get; set; } = null!;

        // Tablica sotrudnikov
        public DbSet<Employee> Employees { get; set; } = null!;

        // Tablica skladskih operacii
        public DbSet<InventoryOperation> InventoryOperations { get; set; } = null!;

        // Tablica zhurnala deistvii
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // Nastroika svyazei i svoistv modelei
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryOperation>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Kaskadnoe udalenie

            modelBuilder.Entity<InventoryOperation>()
                .HasOne(op => op.Employee)
                .WithMany()
                .HasForeignKey(op => op.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull); // Ustanovit NULL pri udalenii sotrudnika

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // 18 vsego znakov, iz nih 2 posle zapyatoi
        }
    }
}