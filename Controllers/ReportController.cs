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
        {
            try // Try-catch block for error handling
            {
                var result = _reportService.GetBestSellingProducts(startDate, endDate, limit); // Call the service method
                return Ok(result); // Return 200 OK with the result
            }
            catch (Exception ex) // Catch any exceptions
            {
                return StatusCode(500, $"An error occurred while retrieving best selling products: {ex.Message}"); // Return 500 Internal Server Error with the exception message
            }

        }

        [HttpGet("RevenueReport")] // Endpoint to get revenue report
        public IActionResult GetRevenueReport( // Get revenue report within a date range
            [FromQuery] DateTime startDate, // Start date from query parameters
            [FromQuery] DateTime endDate, // End date from query parameters
            [FromQuery] string periodType = "daily") // Period type (daily, weekly, monthly)
        {
            try // Try-catch block for error handling
            {
                if (periodType.ToLower() != "daily" && periodType.ToLower() != "monthly") // Validate period type
                {
                    return BadRequest("Period type must be 'daily' or 'monthly'.");
                }

                var result = _reportService.GetRevenueReport(startDate, endDate, periodType); // Call the service method
                return Ok(result);
            }
            catch (Exception ex) // Catch any exceptions

            {
                return StatusCode(500, $"An error occurred while retrieving revenue report: {ex.Message}");
            }

        }
        [HttpGet("TopRatedProducts")] // Endpoint to get top-rated products
        public IActionResult GetTopRatedProducts([FromQuery] int limit = 10) // Get top-rated products
        {
            try // Try-catch block for error handling
            {
                var result = _reportService.GetTopRatedProducts(limit);
                return Ok(result);
            }
            catch (Exception ex) // Catch any exceptions
            {
                return StatusCode(500, $"An error occurred while retrieving top rated products: {ex.Message}");
            }

        }

        [HttpGet("ActiveCustomers")] // Endpoint to get active customers
        public IActionResult GetActiveCustomers( // Get most active customers within a date range
         [FromQuery] DateTime startDate,
         [FromQuery] DateTime endDate,
         [FromQuery] int limit = 10)

    }
}
