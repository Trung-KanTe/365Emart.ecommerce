namespace Commerce.Command.Contract.Abstractions
{
    public interface ILowStockNotifier
    {
        Task NotifyShopsWithLowStockAsync();
    }
}