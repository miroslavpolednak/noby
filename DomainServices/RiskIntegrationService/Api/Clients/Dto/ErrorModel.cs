namespace DomainServices.RiskIntegrationService.Api.Clients.Dto;

internal class ErrorModel
{
    public int Category { get; set; }
    public string? Code { get; set; }
    public string? Message { get; set; }
}
