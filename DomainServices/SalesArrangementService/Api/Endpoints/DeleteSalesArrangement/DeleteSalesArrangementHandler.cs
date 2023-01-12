﻿using CIS.Foms.Enums;
using Microsoft.EntityFrameworkCore;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : IRequestHandler<DeleteSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteSalesArrangementRequest request, CancellationToken cancellation)
    {
        var saInstance = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        if (!request.HardDelete)
        {
            // kontrola na kategorii
            if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == saInstance.SalesArrangementTypeId).SalesArrangementCategory != 2)
                throw new CisValidationException(18013, $"SalesArrangement type not supported");

            // kontrola na stav
            if (saInstance.State != (int)SalesArrangementStates.InProgress && saInstance.State != (int)SalesArrangementStates.IsSigned)
                throw new CisValidationException($"SalesArrangement cannot be updated/deleted in this state {saInstance.State}");
        }

        // smazat SA
        _dbContext.Remove(saInstance);
        //TODO predelat v .NET7 na EF nativni delete
        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.SalesArrangementParameters WHERE SalesArrangementId={request.SalesArrangementId}", cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public DeleteSalesArrangementHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}