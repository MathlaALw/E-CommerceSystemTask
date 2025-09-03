using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace E_CommerceSystem.Controllers
{
    [Authorize(Roles = "admin")] // Only admin can access these endpoints
    [ApiController] // Marks this class as an API controller
    [Route("api/[Controller]")] // Base route for the controller

    public class ReportController : ControllerBase // Inherits from ControllerBase
    {
        private readonly IReportService _reportService; // Report service dependency

        public ReportController(IReportService reportService) // Constructor with dependency injection
        {
            _reportService = reportService;
        }

        [HttpGet("BestSellingProducts")] // Endpoint to get best-selling products
        public IActionResult GetBestSellingProducts( // Get best-selling products within a date range
          [FromQuery] DateTime startDate, // Start date from query parameters
          [FromQuery] DateTime endDate, // End date from query parameters
          [FromQuery] int limit = 10) // Limit the number of results


    }
}
