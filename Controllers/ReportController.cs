using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
         [FromQuery] DateTime startDate, // Start date from query parameters
         [FromQuery] DateTime endDate, // End date from query parameters
         [FromQuery] int limit = 10) // Limit the number of results
        {
            try // Try-catch block for error handling
            {
                var result = _reportService.GetMostActiveCustomers(startDate, endDate, limit); // Call the service method
                return Ok(result); // Return 200 OK with the result
            }
            catch (Exception ex) // Catch any exceptions
            {
                return StatusCode(500, $"An error occurred while retrieving active customers: {ex.Message}");
            }

        }

        [HttpGet("DashboardSummary")] // Endpoint to get dashboard summary
        public IActionResult GetDashboardSummary()
        {
            try // Try-catch block for error handling
            {
                var today = DateTime.Today; // Today's date
                var monthStart = new DateTime(today.Year, today.Month, 1); // First day of the month
                var monthEnd = monthStart.AddMonths(1).AddDays(-1); // Last day of the month

                // Today's revenue
                var todayRevenue = _reportService.GetRevenueReport(today, today, "daily") // Call the service method
                    .FirstOrDefault()?.TotalRevenue ?? 0; // Get the total revenue or 0 if null

                // This month's revenue
                var monthRevenue = _reportService.GetRevenueReport(monthStart, monthEnd, "daily") // Call the service method
                    .Sum(r => r.TotalRevenue); // Sum the total revenue

                // Total customers
                var totalCustomers = _context.Users.Count(u => u.Role == "user"); // Count users with role 'user'

                // Total orders this month
                var monthOrders = _context.Orders
                    .Count(o => o.OrderDate >= monthStart && o.OrderDate <= monthEnd && o.Status != OrderStatus.Cancelled);






            }
    }
    }
