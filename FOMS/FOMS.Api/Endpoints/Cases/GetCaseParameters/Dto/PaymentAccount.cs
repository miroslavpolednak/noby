namespace FOMS.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class PaymentAccount
{
    public string? Prefix { get; set; }

    public string? Number { get; set; }

    public string? BankCode { get; set; }
}