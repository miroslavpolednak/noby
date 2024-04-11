using DomainServices.CaseService.Contracts;

namespace NOBY.Services.MortgageRefinancing;

public sealed class GetRefinancingDataResult
{
    public DomainServices.SalesArrangementService.Contracts.SalesArrangement? SalesArrangement { get; set; }
    
    public RefinancingStates RefinancingState { get; set; }

    public List<NOBY.Dto.Workflow.WorkflowTask>? Tasks { get; set; }

    public ProcessTask? Process { get; set; }

    public int? ActivePriceExceptionTaskIdSb { get; set; }
}
