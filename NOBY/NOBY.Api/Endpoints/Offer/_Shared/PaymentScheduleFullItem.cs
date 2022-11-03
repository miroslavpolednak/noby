namespace NOBY.Api.Endpoints.Offer.Dto;

public class PaymentScheduleFullItem
{
    public string? PaymentNumber { get; set; }
    public string? Date { get; set; }
    public string? Amount { get; set; }
    public string? Principal { get; set; }
    public string? Interest { get; set; }
    public string? RemainingPrincipal { get; set; }
}
