﻿using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementMediatrRequest
    : IRequest<SalesArrangement>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }

    public GetSalesArrangementMediatrRequest(SalesArrangementIdRequest request)
    {
        SalesArrangementId = request.SalesArrangementId;
    }
}
