namespace NOBY.Api.Endpoints.Offer.SharedDto;

public sealed class GetFullPaymentScheduleResponse
{
    /// <summary>
    /// Položky splátkového kalendáře.
    /// </summary>
    public List<PaymentScheduleFullItem>? Items { get; set; }
}
