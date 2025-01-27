﻿using DomainServices.RealEstateValuationService.Api.Database;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetRealEstateValuationDetailByOrderId;

internal sealed class GetRealEstateValuationDetailByOrderIdHandler(
    RealEstateValuationServiceDbContext _dbContext,
    IMediator _mediator)
        : IRequestHandler<__Contracts.GetRealEstateValuationDetailByOrderIdRequest, __Contracts.RealEstateValuationDetail>
{
    public async Task<__Contracts.RealEstateValuationDetail> Handle(__Contracts.GetRealEstateValuationDetailByOrderIdRequest request, CancellationToken cancellationToken)
    {
        var id = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.OrderId == request.OrderId)
            .Select(t => new { t.RealEstateValuationId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.OrderId);

        return await _mediator.Send(new __Contracts.GetRealEstateValuationDetailRequest
        {
            RealEstateValuationId = id.RealEstateValuationId
        }, cancellationToken);
    }
}
