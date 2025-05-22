namespace Commerce.Command.Contract.Abstractions
{
    public interface IOrderService
    {
        Task CancelPendingOrdersAsync();
    }
}