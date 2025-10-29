﻿using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Baskets;
using Store.G02.Domain.Exceptions.BadRequest;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Services.Abstraction.Baskets;
using Store.G02.Shared.Dtos.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Baskets
{
    internal class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {

        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundException(id);
            var result = _mapper.Map<BasketDto>(basket);
            return result;
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
            basket = await _basketRepository.UpdateBasketAsync(basket);
            if (basket is null) throw new BasketCreateOrUpdateBadRequestException();
            var result = _mapper.Map<BasketDto>(basket);
            return result;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var flage = await _basketRepository.DeleteBasketAsync(id);
            if(flage == false) throw new BasketDeleteBadRequestException();
            return flage;
        }

    }
}
