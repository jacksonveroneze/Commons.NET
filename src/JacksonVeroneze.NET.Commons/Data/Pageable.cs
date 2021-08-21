using System.Collections.Generic;

namespace JacksonVeroneze.NET.Commons.Data
{
    public class Pageable<T>
    {
        public int? Total { get; set; }

        public int? Pages { get; set; }

        public int? CurrentPage { get; set; }

        public IList<T> Data { get; set; }
    }
}
