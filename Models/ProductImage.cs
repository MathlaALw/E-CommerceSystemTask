using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class ProductImage
    {
        [Key] // Primary Key
        public int ImageId { get; set; } // Unique identifier for each image

        [Required]
        public string ImageUrl { get; set; }

        public bool IsMain { get; set; }

        public int DisplayOrder { get; set; }

        [ForeignKey("Product")]
        public int PID { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }

    }
}
