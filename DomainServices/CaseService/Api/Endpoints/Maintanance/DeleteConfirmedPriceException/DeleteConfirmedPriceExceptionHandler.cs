using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Api.Endpoints.Maintanance.DeleteConfirmedPriceException;

internal sealed class DeleteConfirmedPriceExceptionHandler(CaseServiceDbContext _dbContext)
    : IRequestHandler<DeleteConfirmedPriceExceptionRequest, Empty>
{
    public async Task<Empty> Handle(DeleteConfirmedPriceExceptionRequest request, CancellationToken cancellationToken)
    {
        await _dbContext
            .ConfirmedPriceExceptions
            .Where(t => t.CaseId == request.CaseId)
            .ExecuteDeleteAsync(cancellationToken);

        return new Empty();
    }
}
