namespace Commerce.Query.Contract.Shared
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(int pageNumber, int pageSize, int totalCount, IEnumerable<T> items) : this(pageNumber, pageSize, totalCount, items.ToArray())
        {

        }

        public PaginatedResult(int pageNumber, int pageSize, int totalCount, T[] items)
        {
            var totalPages = totalCount / (float)pageSize;

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalPages);
            Items = items;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } 
        public int TotalPages { get; set; }
        public T[] Items { get; set; }
    }
}