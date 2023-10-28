namespace DomainServices.ProductService.Api.Database.Models;

internal class Obligation
{
    public long ProductId { get; set; }

    public short LoanPurposeId { get; set; }

    public short ObligationTypeId { get; set; }

    public decimal Amount { get; set; }

    public string? CreditorName { get; set; }

    public string? AccountNumberPrefix { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankCode { get; set; }

    public string? VariableSymbol { get; set; }
}