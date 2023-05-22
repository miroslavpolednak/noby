using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Codebooks.GetAll.Dto;

public class ProductTypeItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? MandantId { get; set; }
    public bool IsValid { get; set; }
    public int Order { get; set; }
    public int? LoanAmountMin { get; set; }
    public int? LoanAmountMax { get; set; }
    public int? LoanDurationMin { get; set; }
    public int? LoanDurationMax { get; set; }
    public int? LtvMin { get; set; }
    public int? LtvMax { get; set; }
    public string? MpHomeApiLoanType { get; set; }
    public int? KonsDbLoanType { get; set; }
    public string? PcpProductId { get; set; }
    public int LoanKindIds { get; set; }

    public List<GenericCodebookFullResponse.Types.GenericCodebookFullItem>? LoanKinds { get; set; }
}
