namespace NOBY.Api.Endpoints.Offer.SharedDto;

public class PaymentScheduleSimpleItem
{
    public string PaymentNumber { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
}