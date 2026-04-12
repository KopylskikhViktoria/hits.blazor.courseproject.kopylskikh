using InventoryControl.Data.Models;

namespace InventoryControl.Data.Interfaces
{
    // Interfeis servisa dlya raboty s dannymi prilozheniya
    public interface IDataService
    {
        // Tovary
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task AddOrUpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);

        // Sotrudniki
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task AddOrUpdateEmployeeAsync(Employee employee);

        // Skladskie operacii
        Task<List<InventoryOperation>> GetAllInventoryOperationsAsync();
        Task AddInventoryOperationAsync(InventoryOperation operation);
        Task DeleteInventoryOperationAsync(int id);
        Task<InventoryOperation?> GetInventoryOperationByIdAsync(int id);
        Task UpdateInventoryOperationAsync(InventoryOperation operation);
    }
}