using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Shared.Dtos.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset OderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
        public decimal SubtTotal { get; set; }
        public decimal MyProperty { get; set; }
    }
}
