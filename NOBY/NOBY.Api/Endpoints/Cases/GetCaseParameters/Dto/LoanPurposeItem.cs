namespace NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class LoanPurposeItem
{
    public DomainServices.CodebookService.Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem? LoanPurpose { get; set; }

    /// <summary>
    /// Výše účelu v Kč
    /// </summary>
    public decimal Sum { get; set; }
}