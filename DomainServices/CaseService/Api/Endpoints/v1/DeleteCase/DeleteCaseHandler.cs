﻿using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.DeleteCase;

internal sealed class DeleteCaseHandler
    : IRequestHandler<DeleteCaseRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCaseRequest request, CancellationToken cancellation)
    {
        // case entity
        var entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // pocet SA na Case
        var count = (await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellation)).SalesArrangements.Count;

        if (count > 0)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.CantDeleteCase);
        }

        // ulozit do DB
        _dbContext.Cases.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public DeleteCaseHandler(
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _salesArrangementService = salesArrangementService;
    }
}
