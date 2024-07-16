using DomainServices.ProductService.Clients;

namespace NOBY.Services.InterestRatesValidFrom;

[SelfService, ScopedService]
public sealed class InterestRatesValidFromService(IProductServiceClient _productService)
{
    public async Task<(DateOnly Date1, DateOnly Date2)> GetValidityDates(long caseId, CancellationToken cancellationToken)
    {
        var mortgage = await _productService.GetMortgage(caseId, cancellationToken);

        var d1 = getFirstDate(mortgage.Mortgage.PaymentDay.GetValueOrDefault());
        
        return (d1, d1.AddMonths(1));
    }

    private static DateOnly getFirstDate(int day)
    {
        var d = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, day);
        var minDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14).Date);
        
        while (d < minDate)
        {
            d = d.AddMonths(1);
        }

        return d;
    }
}
