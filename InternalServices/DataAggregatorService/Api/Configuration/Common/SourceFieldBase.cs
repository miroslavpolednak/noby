namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;

internal class SourceFieldBase
{
    public required DataService DataService { get; init; }

    public required string FieldPath { get; init; }
}