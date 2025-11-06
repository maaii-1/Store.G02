using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Orders;
using Store.G02.Domain.Entities.Products;
using Store.G02.Domain.Exceptions.BadRequest;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Services.Abstraction.Orders;
using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            //Get Order Address

            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            // GetDelivery Method By Id 

            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeleviryMehtodNotFoundException(request.DeliveryMethodId);

            // Get Order Items:
            // 1. Get Basket By Id 
            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if(basket is null) throw new BasketNotFoundException(request.BasketId);

            // 2. Convert Every Basket Item To order Item
            
            var orderItems = new List<OrderItem>();

            foreach( var item in basket.Items)
            {
                // Check Price
                // Get Product From Database
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if(product is null) throw new ProductNotFoundException(item.Id);

                if(product.Price != item.Price) item.Price = product.Price;

                var prductInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(prductInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // Calculate SubTotal 
            var subTotal = orderItems.Sum(OI => OI.Price *  OI.Quantity);


            // Create Order

            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subTotal);


            // Create Order In DataBase
            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();
            if (count <= 0) throw new CreateOrderBadRequestException();
            return _mapper.Map<OrderResponse>(order);
        }

        public Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponse>> GetOrdersByIdForSpecificUserAsync(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
