namespace E_CommerceSystem.Models
{
    public class RevenueReportDTO
    {
        public DateTime Period { get; set; } // Period (e.g., Month, Year) 
        public string PeriodType { get; set; } // Period Type (e.g., "Monthly", "Yearly")
    }
}
