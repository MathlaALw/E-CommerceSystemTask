using System.IdentityModel.Tokens.Jwt;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IOrderService _orderService;

        public InvoiceController(IInvoiceService invoiceService, IOrderService orderService)
        {
            _invoiceService = invoiceService;
            _orderService = orderService;
        }

        // Generate PDF Invoice for an order
        [HttpGet("Generate/{orderId}")]
        public IActionResult GenerateInvoice(int orderId)
        {
            try
            {
                // Verify the user has access to this order
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = GetUserIdFromToken(token);
                int uid = int.Parse(userId);

                var userOrders = _orderService.GetOrderByUserId(uid);
                if (!userOrders.Any(o => o.OID == orderId))
                {
                    return Unauthorized("You can only generate invoices for your own orders.");
                }

                // Get invoice data and generate PDF
                var invoiceData = _invoiceService.GetInvoiceData(orderId);
                var pdfBytes = _invoiceService.GenerateInvoicePdf(invoiceData);

                // Return PDF file
                return File(pdfBytes, "application/pdf", $"Invoice-{orderId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the invoice: {ex.Message}");
            }
        }

        // Preview Invoice Data for an order
        [HttpGet("Preview/{orderId}")]
        public IActionResult PreviewInvoice(int orderId)
        {
            try
            {
                // Verify the user has access to this order
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = GetUserIdFromToken(token);
                int uid = int.Parse(userId);

                var userOrders = _orderService.GetOrderByUserId(uid);
                if (!userOrders.Any(o => o.OID == orderId))
                {
                    return Unauthorized("You can only preview invoices for your own orders.");
                }

                // Get invoice data
                var invoiceData = _invoiceService.GetInvoiceData(orderId);
                return Ok(invoiceData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving invoice data: {ex.Message}");
            }
        }
        // Extract user ID from JWT token
        private string? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");
                return subClaim?.Value;
            }

            throw new UnauthorizedAccessException("Invalid or unreadable token.");
        }

    }
}
