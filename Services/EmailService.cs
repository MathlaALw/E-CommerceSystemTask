using E_CommerceSystem.Models;
using System.Net.Mail;
using System.Net;

namespace E_CommerceSystem.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public EmailService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // Send Order Configration Email
        public void SendOrderConfirmationEmail(Order order)
        {
            var user = _userService.GetUserById(order.UID);
            var subject = $"Order Confirmation - #{order.OID}";
            var body = $@"
                <h2>Thank you for your order!</h2>
                <p>Your order #{order.OID} has been received and is being processed.</p>
                <p><strong>Order Total:</strong> ${order.TotalAmount}</p>
                <p><strong>Order Date:</strong> {order.OrderDate.ToString("f")}</p>
                <p>We'll notify you when your order ships.</p>
            ";

            SendEmail(user.Email, subject, body);
        }

        // Send order cancellation email
        public void SendOrderCancellationEmail(Order order)
        {
            var user = _userService.GetUserById(order.UID);
            var subject = $"Order Cancelled - #{order.OID}";
            var body = $@"
                <h2>Your order has been cancelled</h2>
                <p>Your order #{order.OID} has been successfully cancelled.</p>
                <p><strong>Refund Amount:</strong> ${order.TotalAmount}</p>
                <p>The refund will be processed within 5-7 business days.</p>
            ";

            SendEmail(user.Email, subject, body);
        }

        // Order status
        public void SendOrderStatusUpdateEmail(Order order, OrderStatus newStatus)
        {
            var user = _userService.GetUserById(order.UID);
            var subject = $"Order Update - #{order.OID}";
            var statusText = newStatus.ToString();

            var body = $@"
                <h2>Your order status has been updated</h2>
                <p>Your order #{order.OID} is now <strong>{statusText}</strong>.</p>
                <p><strong>Order Total:</strong> ${order.TotalAmount}</p>
                <p><strong>Order Date:</strong> {order.OrderDate.ToString("f")}</p>
            ";

            if (newStatus == OrderStatus.Shipped)
            {
                body += "<p>Your order has been shipped and is on its way!</p>";
            }
            else if (newStatus == OrderStatus.Delivered)
            {
                body += "<p>Your order has been delivered. Thank you for shopping with us!</p>";
            }

            SendEmail(user.Email, subject, body);
        }

        // Sending Email
        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpSettings["FromEmail"];
                var password = smtpSettings["Password"];
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new NetworkCredential(fromEmail, password);
                    client.EnableSsl = enableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log email sending error
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        // Send invoice email with PDF attachment
        public void SendInvoiceEmail(Order order, byte[] invoicePdf)
        {
            var user = _userService.GetUserById(order.UID);
            var subject = $"Your Invoice - Order #{order.OID}";
            var body = $@"
        <h2>Your Order Invoice</h2>
        <p>Attached is the invoice for your order #{order.OID}.</p>
        <p><strong>Order Total:</strong> ${order.TotalAmount}</p>
        <p><strong>Order Date:</strong> {order.OrderDate.ToString("f")}</p>
        <p>Thank you for shopping with us!</p>
    ";

            SendEmailWithAttachment(user.Email, subject, body, invoicePdf, $"Invoice-{order.OID}.pdf");
        }

        private void SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpSettings["FromEmail"];
                var password = smtpSettings["Password"];
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new NetworkCredential(fromEmail, password);
                    client.EnableSsl = enableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    // Add attachment
                    using (var stream = new MemoryStream(attachment))
                    {
                        var attachmentItem = new Attachment(stream, attachmentName, "application/pdf");
                        mailMessage.Attachments.Add(attachmentItem);

                        client.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log email sending error
                Console.WriteLine($"Failed to send email with attachment: {ex.Message}");
            }
        }
    }
}

