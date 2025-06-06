﻿using Asp.Versioning;
using Commerce.Query.Application.DistrictCases.District;
using Commerce.Query.Application.UserCases.Ward;
using Commerce.Query.Application.WardCases.Ward;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Ward
{
    /// <summary>
    /// Controller version 1 for ward apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.WARD_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class wardService : ApiController
    {
        private readonly IMediator mediator;

        public wardService(IMediator mediator)
        {
            this.mediator = mediator;
        }
     

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllWards()
        {
            // Tạo query phân trang
            var query = new GetAllWardQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }

        [MapToApiVersion(1)]
        [HttpGet("district/{districtId:int}")]
        public async Task<IActionResult> GetDistrictById(int? districtId)
        {
            var query = new GetWardByIdQuery
            {
                DistrictId = districtId
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [MapToApiVersion(1)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetailWard(int id)
        {
            var query = new GetDetailWardQuery
            {
                Id = id
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}