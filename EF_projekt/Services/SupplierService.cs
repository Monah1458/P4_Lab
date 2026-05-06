using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) throw new KeyNotFoundException($"Supplier {id} not found.");
            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _unitOfWork.Suppliers.GetAllAsync();
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                throw new ArgumentException("Supplier name required.");
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.CompleteAsync();       
            return supplier;
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            var existing = await GetSupplierByIdAsync(supplier.IdSupplier);
            existing.SupplierName = supplier.SupplierName;
            _unitOfWork.Suppliers.Update(existing);
            await _unitOfWork.CompleteAsync();
            
        }

        public async Task DeleteSupplierAsync(int id)
        {
            await GetSupplierByIdAsync(id);
            await _unitOfWork.Suppliers.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }

    }
}
