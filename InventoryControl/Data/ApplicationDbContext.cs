using InventoryControl.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<InventoryOperation> InventoryOperations { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryOperation>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // ��������� ��������

            modelBuilder.Entity<InventoryOperation>()
                .HasOne(op => op.Employee)
                .WithMany()
                .HasForeignKey(op => op.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull); // ���������� NULL ��� �������� ����������

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // 18 ����� ������, �� ��� 2 ����� �������
        }
    }
}
