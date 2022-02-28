using System.Collections.Generic;

namespace JacksonVeroneze.NET.Commons.Data
{
    public class PageResult<T>
    {
        public IList<T> Data { get; set; }

        public int? TotalElements { get; set; }
        
        public int? TotalPages { get; set; }

        public int? CurrentPage { get; set; }
        
        public int? PageSize { get; set; }
    }
}
