using EF_projekt.Relations;

namespace EF_projekt.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(int id);
    }
}