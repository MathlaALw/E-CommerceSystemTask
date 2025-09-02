using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models
{
    public class SupplierDTO
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        public string Phone { get; set; }
    }
}
