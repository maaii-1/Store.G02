using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstraction;
using Store.G02.Shared.Dtos.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBasketById(BasketDto basketDto)
        {
            var result = await _serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
