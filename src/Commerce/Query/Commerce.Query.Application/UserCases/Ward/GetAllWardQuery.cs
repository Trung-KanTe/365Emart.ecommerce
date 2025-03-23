using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Application.UserCases.Ward
{
    /// <summary>
    /// Request to get all ward
    /// </summary>
    public class GetAllWardQuery : IRequest<Result<List<WardDTO>>>
    {
    }

    /// <summary>
    /// Handler for get all ward request
    /// </summary>
    public class GetAllWardQueryHandler : IRequestHandler<GetAllWardQuery, Result<List<WardDTO>>>
    {
        private readonly IWardRepository wardRepository;
        private readonly IWebWardService webWardService;

        /// <summary>
        /// Handler for get all ward request
        /// </summary>
        public GetAllWardQueryHandler(IWardRepository wardRepository, IWebWardService webWardService)
        {
            this.wardRepository = wardRepository;
            this.webWardService = webWardService;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list ward as data</returns>
        public async Task<Result<List<WardDTO>>> Handle(GetAllWardQuery request,
                                                       CancellationToken cancellationToken)
        {
            var wards = wardRepository.FindAll().ToList();
            var userDTOs = wards.Select(user => new WardDTO
            {
                Id = user.Id,
                Name = user.Name,
                FullName = user.FullName,
            }).ToList();

            // Lấy danh sách wardId duy nhất
            var wardIds = userDTOs
                .Where(user => user.Id.HasValue)
                .Select(user => user.Id.Value)
                .Distinct()
                .ToArray();

            // Gọi service để lấy thông tin địa phương
            var localizations = await webWardService.GetLocalFullsByWardIds(wardIds);
            var localizationDetails = await webWardService.GetLocalDetailsByWardIds(wardIds);

            // Map dữ liệu LocalizationFullDTO vào từng user
            foreach (var user in userDTOs)
            {
                var localization = localizations.FirstOrDefault(l => l.WardId == user.Id);
                user.LocalizationFullDTO = localization;
                var localizationDetail = localizationDetails.FirstOrDefault(l => l.Ward.Id == user.Id);
                user.LocalizationDetailDTO = localizationDetail;
            }
            return userDTOs;
        }
    }
}
