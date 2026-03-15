using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Common
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageIndex > 1; 
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedResult()
        {
            Items = new List<T>();
        }

        public PaginatedResult(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
