using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class ExtraPaymentBuilder(BuilderValidatorAggregate aggregate) : BaseBuilder(aggregate)
{
    public override async Task<DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        var caseService = GetRequiredService<ICaseServiceClient>();

        try
        {
            var wfTaskRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
            {
                CaseId = Request.CaseId,
                TaskTypeId = (int)WorkflowTaskTypes.ExtraPayment
            };

            var response = await caseService.CreateTask(wfTaskRequest, cancellationToken);

            Request.ProcessId = response.TaskId;
            Request.ExtraPayment = new SalesArrangementParametersExtraPayment();
        }
        catch (CisValidationException ex) when (ex.FirstExceptionCode == "13037")
        { 
            throw new NobyValidationException(90052);
        }

        return Request;
    }
}