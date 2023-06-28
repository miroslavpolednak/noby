namespace NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class LoanPurposeItem
{
    public DomainServices.CodebookService.Contracts.IBaseCodebook? LoanPurpose { get; set; }

    /// <summary>
    /// Výše účelu v Kč
    /// </summary>
    public decimal Sum { get; set; }
}