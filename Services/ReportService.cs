using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace E_CommerceSystem.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context; // Database context

        public ReportService(ApplicationDbContext context) // Constructor with dependency injection
        {
            _context = context;
        }

        public IEnumerable<BestSellingProductDTO> GetBestSellingProducts(DateTime startDate, DateTime endDate, int limit = 10) // Get best-selling products within a date range
        {
            return _context.OrderProducts // Query to get best-selling products
             .Include(op => op.product) // Include product details
             .Include(op => op.Order) // Include order details
             .Where(op => op.Order.OrderDate >= startDate && op.Order.OrderDate <= endDate) // Filter by date range
             .GroupBy(op => new { op.PID, op.product.ProductName }) // Group by product ID and name
             .Select(g => new BestSellingProductDTO // Select into DTO
             {
                 ProductId = g.Key.PID, // Product ID
                 ProductName = g.Key.ProductName, // Product Name
                 TotalQuantitySold = g.Sum(op => op.Quantity), // Total Quantity Sold
                 TotalRevenue = g.Sum(op => op.Quantity * op.product.Price) // Total Revenue
             })
              .OrderByDescending(p => p.TotalQuantitySold) // Order by total quantity sold descending
             .Take(limit) // Limit the number of results
             .ToList(); // Execute the query and return the list
        }


        public IEnumerable<RevenueReportDTO> GetRevenueReport(DateTime startDate, DateTime endDate, string periodType = "daily") // Get revenue report within a date range
        {
            var orders = _context.Orders // Query to get orders
               .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status != OrderStatus.Cancelled) // Filter by date range and exclude cancelled orders
               .ToList(); // Execute the query and get the list

            if (periodType.ToLower() == "daily") // Daily report
            {
                return orders // Query to group by day
                    .GroupBy(o => o.OrderDate.Date) // Group by order date (day)
                    .Select(g => new RevenueReportDTO // Select into DTO
                    {
                        Period = g.Key, // Period (day)
                        PeriodType = "Daily",
                        TotalOrders = g.Count(), // Total number of orders
                        TotalRevenue = g.Sum(o => o.TotalAmount), // Total revenue
                        AverageOrderValue = g.Average(o => o.TotalAmount) // Average order value
                    })
                    .OrderBy(r => r.Period) // Order by period
                    .ToList(); // Execute the query and return the list
            }

            else // monthly
            {
                return orders // Query to group by month
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month }) // Group by year and month
                    .Select(g => new RevenueReportDTO // Select into DTO
                    {
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1),
                        PeriodType = "Monthly",
                        TotalOrders = g.Count(),  // Total number of orders
                        TotalRevenue = g.Sum(o => o.TotalAmount), // Total revenue
                        AverageOrderValue = g.Average(o => o.TotalAmount) // Average order value
                    })
                    .OrderBy(r => r.Period) // Order by period
                    .ToList(); // Execute the query and return the list
            }
        }
        // Get top-rated products
        public IEnumerable<TopRatedProductDTO> GetTopRatedProducts(int limit = 10) 
        {
            return _context.Products // Query to get products
                .Where(p => p.Reviews.Count > 0) // Filter products with at least one review
                .Select(p => new TopRatedProductDTO // Select into DTO
                {
                    ProductId = p.PID, // Product ID
                    ProductName = p.ProductName, // Product Name
                    AverageRating = p.OverallRating, // Average Rating
                    ReviewCount = p.Reviews.Count // Number of Reviews
                })
                .OrderByDescending(p => p.AverageRating) // Order by average rating descending
                .ThenByDescending(p => p.ReviewCount) // Then by review count descending
                .Take(limit) // Limit the number of results
                .ToList(); // Execute the query and return the list
        }

        public IEnumerable<ActiveCustomerDTO> GetMostActiveCustomers(DateTime startDate, DateTime endDate, int limit = 10) // Get most active customers within a date range
        {
            return _context.Orders // Query to get orders
                .Include(o => o.user) // Include user details
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status != OrderStatus.Cancelled) // Filter by date range and exclude cancelled orders
                .GroupBy(o => new { o.UID, o.user.UName, o.user.Email })
                .Select(g => new ActiveCustomerDTO
                {
                    UserId = g.Key.UID,
                    UserName = g.Key.UName,
                    Email = g.Key.Email,
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(c => c.TotalSpent)
                .ThenByDescending(c => c.OrderCount)
                .Take(limit)
                .ToList();
        }
    }
}













    }


}


