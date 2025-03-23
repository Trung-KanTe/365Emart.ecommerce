namespace Commerce.Query.Contract.Helpers
{
    public class SearchCommand
    {
        public List<FilterParam> FilterParams { get; set; } = new List<FilterParam>();
        public List<OrderParam> OrderParams { get; set; } = new List<OrderParam>();
    }
}
