using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Abstraction.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest order, string userEmail);
        Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync();
        Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string userEmail);
        Task<IEnumerable<OrderResponse>> GetOrdersByIdForSpecificUserAsync(string userEmail);
    }
}
