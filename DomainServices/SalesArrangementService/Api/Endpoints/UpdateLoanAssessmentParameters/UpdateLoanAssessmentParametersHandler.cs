using CIS.Foms.Enums;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateLoanAssessmentParameters;

internal sealed class UpdateLoanAssessmentParametersHandler
    : IRequestHandler<UpdateLoanAssessmentParametersRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateLoanAssessmentParametersRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        entity.LoanApplicationAssessmentId = request.LoanApplicationAssessmentId;
        entity.RiskSegment = request.RiskSegment;
        entity.CommandId = request.CommandId;
        entity.RiskBusinessCaseExpirationDate = request.RiskBusinessCaseExpirationDate;

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)SalesArrangementStates.NewArrangement)
        {
            entity.State = (int)SalesArrangementStates.InProgress;
        }

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly SalesArrangementServiceDbContext _dbContext;

    public UpdateLoanAssessmentParametersHandler(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
