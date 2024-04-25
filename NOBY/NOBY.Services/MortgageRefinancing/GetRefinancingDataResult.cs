using DomainServices.CaseService.Contracts;

namespace NOBY.Services.MortgageRefinancing;

public sealed class GetRefinancingDataResult
{
    public DomainServices.SalesArrangementService.Contracts.SalesArrangement? SalesArrangement { get; set; }
    
    /// <summary>
    /// Napocitany stav procesu v NOBY
    /// </summary>
    public RefinancingStates RefinancingState { get; set; }

    /// <summary>
    /// Vsechny tasky vcetne IC na procesu
    /// </summary>
    public List<NOBY.Dto.Workflow.WorkflowTask>? Tasks { get; set; }

    /// <summary>
    /// Detail refinancniho procesu
    /// </summary>
    public ProcessTask? Process { get; set; }

    /// <summary>
    /// Aktivni cenova vyjimka
    /// </summary>
    public AmendmentPriceException? ActivePriceException { get; set; }
}
