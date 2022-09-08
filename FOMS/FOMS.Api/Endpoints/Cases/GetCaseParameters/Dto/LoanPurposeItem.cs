namespace FOMS.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class LoanPurposeItem
{
    public DomainServices.CodebookService.Contracts.GenericCodebookItem? LoanPurpose { get; set; }
    
    public decimal Sum { get; set; }
}