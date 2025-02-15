using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Messages;
using Commerce.Query.Contract.Shared;
using MassTransit;
using System.Text.Json;

namespace Commerce.Query.Contract.Services
{
    /// <summary>
    /// Service help to handle files
    /// </summary>
    public class WebWardServiceRabbit : IWebWardService
    {
        private readonly IRequestClient<GetWebLocalByWardIdsRequest> getWebLocalByWardIdsClient;      

        public WebWardServiceRabbit(IRequestClient<GetWebLocalByWardIdsRequest> getWebLocalByWardIdsClient)
        {
            this.getWebLocalByWardIdsClient = getWebLocalByWardIdsClient;
        }

        public async Task<List<LocalizationFullDTO>> GetLocalFullsByWardIds(params int[] wardIds)
        {
            try
            {
                // Create request
                GetWebLocalByWardIdsRequest request = new()
                {
                    WardIds = wardIds.ToList()
                };

                // Get response
                GetWebLocalByWardIdsResponse response = (await getWebLocalByWardIdsClient.GetResponse<GetWebLocalByWardIdsResponse>(request)).Message;              

                return JsonSerializer.Deserialize<List<LocalizationFullDTO>>(response.Data!)!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }       
    }
}