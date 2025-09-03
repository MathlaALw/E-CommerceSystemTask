using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ISupplierService
    {
        void AddSupplier(SupplierDTO supplierDTO);
        void DeleteSupplier(int id);
        List<SupplierDTO> GetAllSuppliers();
        SupplierDTO GetSupplierById(int id);
        void UpdateSupplier(int id, SupplierDTO supplierDTO);
    }
}