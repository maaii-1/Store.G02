using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Services.Abstraction;
using Store.G02.Services.Abstraction.Products;
using Store.G02.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services
{
    internal class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper) : IServiceManager
    {
        public IProductService productService { get; } = new ProductService(_unitOfWork, _mapper);
    }
}
