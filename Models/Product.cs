using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class Product
    {
        [Key]
        public int PID { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public decimal OverallRating { get; set; }

        // Add image support
        public string MainImageUrl { get; set; }

        [JsonIgnore] // To prevent circular references during serialization
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderProducts> OrderProducts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Review> Reviews { get; set; }

        // Navigation property for Category
        [ForeignKey("Categoty")]
        public int CategoryId { get; set; }

        public Category Categoty{get; set;}

        // Navigation property for Supplier
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }


    }
}
