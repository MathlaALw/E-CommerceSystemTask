namespace E_CommerceSystem.Models
{
    public class TopRatedProductDTO
    {
        public int ProductId { get; set; } // Product ID
        public string ProductName { get; set; } // Product Name
        public decimal AverageRating { get; set; } // Average Rating
        public int ReviewCount { get; set; } // Number of Reviews

    }
}
