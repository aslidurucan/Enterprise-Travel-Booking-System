using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Caching
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
