namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class RiskLoanApplicationDataField
{
    public int DataFieldId { get; set; }

    public string JsonPropertyName { get; set; } = null!;

    public DataField DataField { get; set; } = null!;

    public bool UseDefaultInsteadOfNull { get; set; }
}