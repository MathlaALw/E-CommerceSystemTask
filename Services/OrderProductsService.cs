using AutoMapper;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class OrderProductsService : IOrderProductsService
    {
        private readonly IOrderProductsRepo _orderProductsRepo;
        private readonly IMapper _mapper; //add AutoMapper dependency injection field
        public OrderProductsService(IOrderProductsRepo orderProductsRepo, IMapper mapper)
        {
            _orderProductsRepo = orderProductsRepo;
            _mapper = mapper; 
        }

        public void AddOrderProducts(OrderProducts product)
        {
            _orderProductsRepo.AddOrderProducts(product);
        }

        public IEnumerable<OrderProducts> GetAllOrders()
        {
            return _orderProductsRepo.GetAllOrders();
        }

        public List<OrderProducts> GetOrdersByOrderId(int oid)
        {
            return _orderProductsRepo.GetOrdersByOrderId(oid);
        }
    }
}
