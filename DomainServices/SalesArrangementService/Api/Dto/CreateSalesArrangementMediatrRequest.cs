﻿using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class CreateSalesArrangementMediatrRequest
    : IRequest<CreateSalesArrangementResponse>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int SalesArrangementType { get; init; }
    public int? OfferInstanceId { get; init; }
    public int? State { get; init; }
    
    public CreateSalesArrangementMediatrRequest(CreateSalesArrangementRequest request)
    {
        CaseId = request.CaseId;
        SalesArrangementType = request.SalesArrangementType;
        OfferInstanceId = request.OfferInstanceId;
        State = request.State;
    }
}
