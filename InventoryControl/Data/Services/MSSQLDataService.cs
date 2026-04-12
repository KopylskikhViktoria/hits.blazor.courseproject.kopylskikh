using InventoryControl.Data.Interfaces;
using InventoryControl.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Services
{
    // Servis dlya raboty s bazoi dannyh cherez EF Core
    public class MSSQLDataService : IDataService
    {
        // Kontekst bazy dannyh
        private readonly ApplicationDbContext _db;

        // Vnedrenie konteksta cherez konstruktor
        public MSSQLDataService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Poluchit vse operacii vmeste s tovarom i sotrudnikom
        public async Task<List<InventoryOperation>> GetAllInventoryOperationsAsync()
        {
            return await _db.InventoryOperations
                .Include(op => op.Product)
                .Include(op => op.Employee)
                .OrderByDescending(op => op.Date)
                .ToListAsync();
        }

        // Poluchit vse tovary
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _db.Products.ToListAsync();
        }

        // Poluchit tovar po Id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        // Dobavit novyi tovar ili obnovit sushchestvuyushchii
        public async Task AddOrUpdateProductAsync(Product product)
        {
            if (product.Id == 0)
                _db.Products.Add(product);
            else
                _db.Products.Update(product);

            await _db.SaveChangesAsync();
        }

        // Udalit tovar po Id
        public async Task DeleteProductAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }

        // Poluchit vseh sotrudnikov
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _db.Employees.ToListAsync();
        }

        // Dobavit skladskuyu operaciyu
        public async Task AddInventoryOperationAsync(InventoryOperation operation)
        {
            // Proverka sushchestvovaniya tovara
            var product = await _db.Products.FindAsync(operation.ProductId);
            if (product == null)
                throw new ArgumentException("Tovar ne naiden");

            _db.InventoryOperations.Add(operation);

            // Audit log - sluzhebnyi zhurnal deistvii
            _db.AuditLogs.Add(new AuditLog
            {
                Action = "Dobavlenie",
                Entity = $"Operation (ProductId: {operation.ProductId}, Qty: {operation.Quantity})",
                Date = DateTime.Now,
                User = "admin"
            });

            await _db.SaveChangesAsync();
        }

        // Poluchit sotrudnika po Id
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _db.Employees.FindAsync(id);
        }

        // Dobavit novogo sotrudnika ili obnovit sushchestvuyushchego
        public async Task AddOrUpdateEmployeeAsync(Employee employee)
        {
            if (employee.Id == 0)
            {
                _db.Employees.Add(employee);
            }
            else
            {
                _db.Employees.Update(employee);
            }

            await _db.SaveChangesAsync();
        }

        // Udalit operaciyu po Id
        public async Task DeleteInventoryOperationAsync(int id)
        {
            var op = await _db.InventoryOperations.FindAsync(id);
            if (op != null)
            {
                _db.InventoryOperations.Remove(op);
                await _db.SaveChangesAsync();
            }
        }

        // Poluchit operaciyu po Id vmeste so svyazannymi dannymi
        public async Task<InventoryOperation?> GetInventoryOperationByIdAsync(int id)
        {
            return await _db.InventoryOperations
                .Include(o => o.Product)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // Obnovit operaciyu
        public async Task UpdateInventoryOperationAsync(InventoryOperation operation)
        {
            _db.InventoryOperations.Update(operation);
            await _db.SaveChangesAsync();
        }
    }
}