using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class ProductImage
    {
        [Key] // Primary Key
        public int ImageId { get; set; } // Unique identifier for each image

        [Required] // URL or path to the image
        public string ImageUrl { get; set; } // Image URL or path

        public bool IsMain { get; set; } // Indicates if this is the main image for the product

        public int DisplayOrder { get; set; } // Order of display if multiple images exist

        [ForeignKey("Product")] // Foreign key to the Product entity
        public int PID { get; set; } // Foreign key to Product

        [JsonIgnore] // To prevent circular references during serialization
        public Product Product { get; set; } // Navigation property to the Product

    }
}
