namespace FOMS.Api.Endpoints.Offer.Dto;

public sealed class GetFullPaymentScheduleResponse
{
    /// <summary>
    /// Položky splátkového kalendáře.
    /// </summary>
    public List<PaymentScheduleFullItem>? Items { get; set; }
}
