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
    }
}
