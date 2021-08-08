using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Services
{
    public class PagedResult<T>
    {
        public IList<T> List { get; set; }
        public PagingInfo Info { get; set; }
    }

    public class PagingInfo
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
    }
}
