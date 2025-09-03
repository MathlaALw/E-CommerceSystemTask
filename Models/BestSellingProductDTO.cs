namespace E_CommerceSystem.Models
{
    public class BestSellingProductDTO
    {
        public int ProductId { get; set; } // Product ID

        public string ProductName { get; set; } // Product Name

        public int TotalQuantitySold { get; set; } // Total Quantity Sold

        public decimal TotalRevenue { get; set; } // Total Revenue Generated

    }
}
