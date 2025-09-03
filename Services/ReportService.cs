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
                .Select(g => new BestSellingProductDTO

        }


    }
}
