using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Orders;
using Store.G02.Domain.Entities.Products;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Services.Abstraction.Payments;
using Store.G02.Shared.Dtos.Baskets;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.G02.Domain.Entities.Products.Product;

namespace Store.G02.Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IConfiguration configuration, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string basketId)
        {
            // Claculate Amount = SubTotal + Delivery Method Cost

            //Get Basket By Id
            var basket =  await _basketRepository.GetBasketAsync(basketId);
            if(basket is  null) throw new BasketNotFoundException(basketId);


            //Check Products And Their Prices
            foreach(var item  in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
            }

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            if (!basket.DeliveryMethodId.HasValue) throw new DeleviryMehtodNotFoundException(-1);

            // Get Delivery Method By Id
            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null) throw new DeleviryMehtodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;

            var amount = subTotal + deliveryMethod.Price;


            // Send Amount TO Stripe
            StripeConfiguration.ApiKey = configuration["StripeOptions:SecretKey"];
            
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;


            if(basket.PaymentIntentId is null)
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)amount * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"}
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);
            }
            else
            {
                // Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)amount * 100,
                };

                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await _basketRepository.UpdateBasketAsync(basket, TimeSpan.FromDays(1));

            return _mapper.Map<BasketDto>(basket);

        }
    }
}

