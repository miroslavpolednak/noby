namespace FOMS.Api.Endpoints.Offer.Dto;

public class PaymentScheduleSimpleItem
{
    public int PaymentIndex { get; set; }
    public string PaymentNumber { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }
    public string Amount { get; set; }
}