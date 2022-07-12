namespace FOMS.Api.Endpoints.Codebooks.GetAll.Dto;

public class ProductTypeItem
    : DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem
{
    public List<DomainServices.CodebookService.Contracts.Endpoints.LoanKinds.LoanKindsItem>? LoanKinds { get; set; }
}
