using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        void AddOrder(OrdersOutputOTD orderDto);
        void CancelOrder(int orderId, int userId);
        void DeleteOrder(int oid);
        List<OrderProducts> GetAllOrders(int uid);
        IEnumerable<OrdersOutputOTD> GetOrderById(int oid, int uid);
        IEnumerable<Order> GetOrderByUserId(int uid);
        void PlaceOrder(List<OrderItemDTO> items, int uid);
        void SendInvoiceEmail(int orderId, int userId);
        void UpdateOrder(Order order);
        void UpdateOrderStatus(int orderId, OrderStatus status, int userId);
    }
}