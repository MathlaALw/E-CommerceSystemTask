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
                    .ToList();
            }








        }


    }
}

