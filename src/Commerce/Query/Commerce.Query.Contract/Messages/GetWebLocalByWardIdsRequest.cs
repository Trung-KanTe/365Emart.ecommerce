using Commerce.Query.Contract.Constants;
using MassTransit;

namespace Commerce.Query.Contract.Messages
{
    [MessageUrn(Const.GET_WEBLOCAL_BY_WARD_IDS_REQUEST_URN)]
    [EntityName(Const.GET_WEBLOCAL_BY_WARD_IDS_REQUEST_ENTITY_NAME)]
    public class GetWebLocalByWardIdsRequest
    {
        public List<int>? WardIds { get; set; }
    }
}