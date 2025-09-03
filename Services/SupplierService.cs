using E_CommerceSystem.Repositories;
using AutoMapper;
using E_CommerceSystem.Models;
namespace E_CommerceSystem.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepo _supplierRepo;
        private readonly IMapper _mapper;
        public SupplierService(ISupplierRepo supplierRepo, IMapper mapper)
        {
            _supplierRepo = supplierRepo;
            _mapper = mapper;
        }

        //Add Supplier
        public void AddSupplier(SupplierDTO supplierDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierDTO);
            _supplierRepo.AddSupplier(supplier);
        }

        // Get All Suppliers
        public List<SupplierDTO> GetAllSuppliers()
        {
            var suppliers = _supplierRepo.GetAllSuppliers();
            return _mapper.Map<List<SupplierDTO>>(suppliers);
        }

        // Get Supplier by ID
        public SupplierDTO GetSupplierById(int id)
        {
            var supplier = _supplierRepo.GetSupplierById(id);
            if (supplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }
            return _mapper.Map<SupplierDTO>(supplier);
        }

        // Update Supplier
        public void UpdateSupplier(int id, SupplierDTO supplierDTO)
        {
            var existingSupplier = _supplierRepo.GetSupplierById(id);
            if (existingSupplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }
            var updatedSupplier = _mapper.Map<Supplier>(supplierDTO);
            updatedSupplier.SupplierId = id; // Ensure the ID remains unchanged
            _supplierRepo.UpdateSupplier(updatedSupplier);
        }

        // Delete Supplier
        public void DeleteSupplier(int id)
        {
            var existingSupplier = _supplierRepo.GetSupplierById(id);
            if (existingSupplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }
            _supplierRepo.DeleteSupplier(id);
        }

    }
}
