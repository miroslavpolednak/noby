using FOMS.Api.Endpoints.Cases.GetCaseParameters.Dto;
using FOMS.Api.SharedDto;

namespace FOMS.Api.Endpoints.Product.GetProductObligationList.Dto;

public class ProductObligation
{
    public int? ProductObligationId { get; set; }
    public int ObligationTypeId { get; set; }
    public decimal Amount { get; set; }
    public string CreditorName { get; set; } = null!;
    public BankAccount PaymentAccount { get; set; } = null!;
    public PaymentSymbols PaymentSymbols { get; set; } = null!;
}