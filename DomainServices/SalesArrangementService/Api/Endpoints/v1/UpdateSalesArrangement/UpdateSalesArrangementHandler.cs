using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal sealed class UpdateSalesArrangementHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)SalesArrangementStates.NewArrangement)
        {
            entity.State = (int)SalesArrangementStates.InProgress;
        }

        // kontrola na stav
        if (!_allowedStates.Contains(entity.State) && entity.SalesArrangementTypeId != (int)SalesArrangementTypes.Mortgage)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SalesArrangementCantDelete, entity.State);
        }

        entity.ContractNumber = !string.IsNullOrWhiteSpace(request.ContractNumber) ? request.ContractNumber : entity.ContractNumber;
        entity.FirstSignatureDate = request.FirstSignatureDate is not null ? request.FirstSignatureDate.ToDateTime() : entity.FirstSignatureDate;
        entity.ProcessId = request.ProcessId.HasValue ? request.ProcessId.Value : null;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static int[] _allowedStates = 
    [ 
        (int)SalesArrangementStates.InSigning, 
        (int)SalesArrangementStates.InProgress 
    ];

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateSalesArrangementHandler(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
