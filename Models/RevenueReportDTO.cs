namespace E_CommerceSystem.Models
{
    public class RevenueReportDTO
    {
        public DateTime Period { get; set; } // Period (e.g., Month, Year) 
        public string PeriodType { get; set; } // Period Type (e.g., "Monthly", "Yearly")
        public int TotalOrders { get; set; } // Total Number of Orders
        public decimal TotalRevenue { get; set; } // Total Revenue Generated
        public decimal AverageOrderValue { get; set; } // Average Order Value
    }
}
