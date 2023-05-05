using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Product.GetProductObligationList.Dto;

public class ProductObligation
{
    public int ObligationTypeId { get; set; }
    public decimal Amount { get; set; }
    public string CreditorName { get; set; } = null!;
    public BankAccount PaymentAccount { get; set; } = null!;
    public PaymentSymbols PaymentSymbols { get; set; } = null!;
}