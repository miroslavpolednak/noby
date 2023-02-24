namespace NOBY.Api.Endpoints.Product.GetProductObligationList.Dto;

public record ProductObligationId
{
    public long LoanId { get; init; }

    public int LoanPurpose { get; init; }
}