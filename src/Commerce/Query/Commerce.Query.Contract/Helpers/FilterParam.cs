namespace Commerce.Query.Contract.Helpers
{
    public class FilterParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Condition { get; set; } // =, >, >=, <, <=, Contains
    }
}
