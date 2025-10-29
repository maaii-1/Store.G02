using Store.G02.Domain.Contracts;
using Store.G02.Services.Abstraction.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Cache
{
    public class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> getAsync(string key)
        {
            var value = await _cacheRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task setAsync(string key, object value, TimeSpan duration)
        {
            await _cacheRepository.SetAsync(key, value, duration);
        }
    }
}
