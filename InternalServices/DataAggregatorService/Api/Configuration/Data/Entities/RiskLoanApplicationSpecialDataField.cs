namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class RiskLoanApplicationSpecialDataField
{
    public string JsonPropertyName { get; set; } = null!;

    public int DataServiceId { get; set; }

    public string FieldPath { get; set; } = null!;

    public DataService DataService { get; set; } = null!;
}