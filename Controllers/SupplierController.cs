using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class SupplierController : ControllerBase
    {
        // inject service
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        // Add Supplier
        [HttpPost("AddSupplier")]
        public IActionResult AddSupplier(SupplierDTO supplierDTO)
        {
            try
            {
                _supplierService.AddSupplier(supplierDTO);
                return Ok("Supplier added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the supplier. {ex.Message}");
            }
        }
        // Update Supplier
        [HttpPut("UpdateSupplier")]
        public IActionResult UpdateSupplier(int supplierId, SupplierDTO supplierDTO)
        {
            try
            {
                _supplierService.UpdateSupplier(supplierId, supplierDTO);
                return Ok("Supplier updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the supplier. {ex.Message}");
            }
        }
        // Delete Supplier
        [HttpDelete("DeleteSupplier")]
        public IActionResult DeleteSupplier(int supplierId)
        {
            try
            {
                _supplierService.DeleteSupplier(supplierId);
                return Ok("Supplier deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the supplier. {ex.Message}");
            }
        }
        // Get All Suppliers
        [HttpGet("GetAllSuppliers")]
        public IActionResult GetAllSuppliers()
        {
            try
            {
                var suppliers = _supplierService.GetAllSuppliers();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving suppliers. {ex.Message}");
            }
        }

        // Get Supplier by ID
        [HttpGet("GetSupplierById")]
        public IActionResult GetSupplierById(int supplierId)
        {
            try
            {
                var supplier = _supplierService.GetSupplierById(supplierId);
                return Ok(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the supplier. {ex.Message}");
            }
        }

    }
}