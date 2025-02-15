using Commerce.Query.Contract.Constants;
using MassTransit;

namespace Commerce.Query.Contract.Messages
{
    [MessageUrn(Const.GET_WEBLOCAL_BY_WARD_IDS_RESPONSE_URN)]
    [EntityName(Const.GET_WEBLOCAL_BY_WARD_IDS_RESPONSE_ENTITY_NAME)]
    public class GetWebLocalByWardIdsResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string? Data { get; set; }
        public string? MessageCode { get; set; }
        public List<string>? ErrorDetail { get; set; }
    }
}