using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal sealed class UpdateSalesArrangementHandler(Database.SalesArrangementServiceDbContext _dbContext)
    : IRequestHandler<Contracts.UpdateSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)EnumSalesArrangementStates.NewArrangement)
        {
            entity.State = (int)EnumSalesArrangementStates.InProgress;
        }

        // kontrola na stav
        if (!_allowedStates.Contains(entity.State) && entity.SalesArrangementTypeId != (int)SalesArrangementTypes.Mortgage)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SalesArrangementCantDelete, entity.State);
        }

        if (!string.IsNullOrWhiteSpace(request.ContractNumber))
        {
            entity.ContractNumber = request.ContractNumber;
        }
        
        if (request.FirstSignatureDate is not null)
        {
            entity.FirstSignatureDate = request.FirstSignatureDate.ToDateTime();
        }
        
        if (request.ProcessId.HasValue)
        {
            entity.ProcessId = request.ProcessId.Value;
        }
        
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static readonly int[] _allowedStates = 
    [ 
        (int)EnumSalesArrangementStates.InSigning, 
        (int)EnumSalesArrangementStates.InProgress 
    ];
}
