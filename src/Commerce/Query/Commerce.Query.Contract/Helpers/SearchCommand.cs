namespace Commerce.Query.Contract.Helpers
{
    public class SearchCommand
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public List<FilterParam> FilterParams { get; set; } = new List<FilterParam>();
        public List<OrderParam> OrderParams { get; set; } = new List<OrderParam>();
    }
}
