namespace E_CommerceSystem.Models
{
    public class ProductImageDTO
    {
        public int ImageId { get; set; } // Unique identifier for each image
        public string ImageUrl { get; set; } // Image URL or path
        public bool IsMain { get; set; } // Indicates if this is the main image for the product
        public int DisplayOrder { get; set; } // Order of display if multiple images exist
    }
}
