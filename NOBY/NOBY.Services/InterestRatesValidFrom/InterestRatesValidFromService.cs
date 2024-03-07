using DomainServices.ProductService.Clients;

namespace NOBY.Services.InterestRatesValidFrom;

[SelfService, ScopedService]
public sealed class InterestRatesValidFromService
{
    public async Task<(DateTime Date1, DateTime Date2)> GetValidityDates(long caseId, CancellationToken cancellationToken)
    {
        var mortgage = await _productService.GetMortgage(caseId, cancellationToken);

        DateTime d1 = getFirstDate(mortgage.Mortgage.PaymentDay.GetValueOrDefault());
        
        return (d1, d1.AddMonths(1));
    }

    private static DateTime getFirstDate(int day)
    {
        var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
        var minDate = DateTime.Now.AddDays(14).Date;

        while (d < minDate)
        {
            d = d.AddMonths(1);
        }

        return d;
    }

    private readonly IProductServiceClient _productService;

    public InterestRatesValidFromService(IProductServiceClient productService)
    {
        _productService = productService;
    }
}
