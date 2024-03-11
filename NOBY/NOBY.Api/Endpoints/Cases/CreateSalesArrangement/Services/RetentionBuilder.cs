using DomainServices.CaseService.Clients.v1;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class RetentionBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        var caseService = GetRequiredService<ICaseServiceClient>();

        // vytvorit WF task
        var wfTaskRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
        {
            CaseId = Request.CaseId,
            TaskTypeId = 9
        };
        var wfResponse = await caseService.CreateTask(wfTaskRequest, cancellationToken);

        Request.Retention = new __SA.SalesArrangementParametersRetention
        {
        };
        Request.TaskProcessId = wfResponse.TaskId;

        return Request;
    }

    public RetentionBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
