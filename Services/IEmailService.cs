using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IEmailService
    {
        void SendOrderCancellationEmail(Order order);
        void SendOrderConfirmationEmail(Order order);
        void SendOrderStatusUpdateEmail(Order order, OrderStatus newStatus);
    }
}