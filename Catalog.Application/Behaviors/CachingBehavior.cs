using Catalog.Application.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;

        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableQuery cacheableQuery)
            {
                var cacheKey = cacheableQuery.CacheKey;

                var cachedResponse = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (cachedResponse != null)
                {
                    _logger.LogInformation($"[CACHE HIT ⚡] Veri yıldırım hızıyla RAM'den getirildi! Anahtar: {cacheKey}");

                    return JsonSerializer.Deserialize<TResponse>(cachedResponse);
                }

                _logger.LogInformation($"[CACHE MISS 🐌] Veri RAM'de yok. Veritabanına iniliyor... Anahtar: {cacheKey}");

                var response = await next();

                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = cacheableQuery.SlidingExpiration
                };

                var serializedData = JsonSerializer.Serialize(response);

                await _cache.SetStringAsync(cacheKey, serializedData, options, cancellationToken);

                return response;
            }

            return await next();
        }
    }
}
