using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public class SupplierRepo : ISupplierRepo
    {
        // injection of context
        public ApplicationDbContext _context;
        public SupplierRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //Add Supplier
        public void AddSupplier(Supplier supplier)
        {
            try
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Update Supplier
        public void UpdateSupplier(Supplier supplier)
        {
            try
            {
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Delete Supplier

        public void DeleteSupplier(int supplierId)
        {
            try
            {
                var supplier = GetSupplierById(supplierId);
                if (supplier != null)
                {
                    _context.Suppliers.Remove(supplier);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get Supplier by Id

        public Supplier GetSupplierById(int supplierId)
        {
            try
            {
                return _context.Suppliers.Find(supplierId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get All Suppliers
        public IEnumerable<Supplier> GetAllSuppliers()
        {
            try
            {
                return _context.Suppliers.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

    }
}
