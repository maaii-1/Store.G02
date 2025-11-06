using Store.G02.Services.Abstraction.Auth;
using Store.G02.Services.Abstraction.Baskets;
using Store.G02.Services.Abstraction.Cache;
using Store.G02.Services.Abstraction.Orders;
using Store.G02.Services.Abstraction.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Abstraction
{
    public interface IServiceManager
    {
        public IProductService productService { get; }

        public IBasketService BasketService { get; }
        public ICacheService CacheService { get; }
        public IAuthService AuthService { get; }
        public IOrderService OrderService { get; }
    }
}
