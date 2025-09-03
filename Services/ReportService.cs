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
        }


    }
}
