using Commerce.Query.Contract.Shared;

namespace Commerce.Query.Contract.Abstractions
{
    /// <summary>
    /// Service help to handle files
    /// </summary>
    public interface IWebWardService
    {
        Task<List<LocalizationFullDTO>> GetLocalFullsByWardIds(params int[] wardIds);
        Task<List<LocalizationDetailDTO>> GetLocalDetailsByWardIds(params int[] wardIds);
    }
}