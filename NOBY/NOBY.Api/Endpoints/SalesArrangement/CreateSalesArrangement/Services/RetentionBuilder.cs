using DomainServices.CaseService.Clients.v1;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class RetentionBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        // vytvorit WF task
        Request.TaskProcessId = await createProcessId(Request.CaseId, cancellationToken);
        
        switch ((SalesArrangementTypes)Request.SalesArrangementTypeId)
        {
            case SalesArrangementTypes.MortgageRetention:
                Request.Retention = new();
                break;
            case SalesArrangementTypes.MortgageRefixation:
                Request.Refixation = new();
                break;
        }

        return Request;
    }

    private async Task<long> createProcessId(long caseId, CancellationToken cancellationToken)
    {
        var caseService = GetRequiredService<ICaseServiceClient>();

        try
        {
            var wfTaskRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
            {
                CaseId = caseId,
                TaskSubtypeId = Request.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRetention ? 1 : 2,
                TaskTypeId = 9
            };
            var response = await caseService.CreateTask(wfTaskRequest, cancellationToken);

            return response.TaskId;
        }
        catch (CisValidationException ex) when (ex.FirstExceptionCode == "13037")
        {
            throw new NobyValidationException(90052);
        }
    }

    public RetentionBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
