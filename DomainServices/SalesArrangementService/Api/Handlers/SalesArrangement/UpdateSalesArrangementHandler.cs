﻿namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementDataHandler
    : IRequestHandler<Dto.UpdateSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        await _repository.UpdateSalesArrangement(request.Request.SalesArrangementId, request.Request.ContractNumber, request.Request.EaCode, request.Request.RiskBusinessCaseId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    
    public UpdateSalesArrangementDataHandler(
        Repositories.SalesArrangementServiceRepository repository)
    {
        _repository = repository;
    }
}
