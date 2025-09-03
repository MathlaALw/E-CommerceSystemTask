using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IReportService
    {
        IEnumerable<BestSellingProductDTO> GetBestSellingProducts(DateTime startDate, DateTime endDate, int limit = 10);
        IEnumerable<ActiveCustomerDTO> GetMostActiveCustomers(DateTime startDate, DateTime endDate, int limit = 10);
        IEnumerable<RevenueReportDTO> GetRevenueReport(DateTime startDate, DateTime endDate, string periodType = "daily");
        IEnumerable<TopRatedProductDTO> GetTopRatedProducts(int limit = 10);
    }
}