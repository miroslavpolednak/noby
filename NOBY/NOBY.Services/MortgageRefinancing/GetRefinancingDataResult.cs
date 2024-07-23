using DomainServices.CaseService.Contracts;
using NOBY.ApiContracts;

namespace NOBY.Services.MortgageRefinancing;

public sealed class GetRefinancingDataResult
{
    public DomainServices.SalesArrangementService.Contracts.SalesArrangement? SalesArrangement { get; set; }
    
    /// <summary>
    /// Napocitany stav procesu v NOBY
    /// </summary>
    public EnumRefinancingStates RefinancingState { get; set; }

    /// <summary>
    /// Vsechny tasky vcetne IC na procesu
    /// </summary>
    public List<SharedTypesWorkflowTask>? Tasks { get; set; }

    /// <summary>
    /// Detail refinancniho procesu
    /// </summary>
    public ProcessTask? Process { get; set; }

    /// <summary>
    /// Aktivni cenova vyjimka
    /// </summary>
    public AmendmentPriceException? ActivePriceException { get; set; }

    /// <summary>
    /// Příznak, jestli je aktivní cenová výjimka aktivní
    /// </summary>
    public bool IsActivePriceExceptionCompleted { get; set; }
}
