using System.Collections.Generic;

namespace BlazorCore.Models
{
    public class PaginatedModel<T>
    {
        public int TotalItemsCount { get; set; }
        public int TotalPageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}
