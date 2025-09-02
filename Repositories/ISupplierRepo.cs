using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface ISupplierRepo
    {
        void AddSupplier(Supplier supplier);
        void DeleteSupplier(int supplierId);
        IEnumerable<Supplier> GetAllSuppliers();
        Supplier GetSupplierById(int supplierId);
        void UpdateSupplier(Supplier supplier);
    }
}