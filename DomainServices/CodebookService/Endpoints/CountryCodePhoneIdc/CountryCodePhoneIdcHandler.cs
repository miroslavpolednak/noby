using DomainServices.CodebookService.Contracts.Endpoints.CountryCodePhoneIdc;

namespace DomainServices.CodebookService.Endpoints.CountryCodePhoneIdc;

public class CountryCodePhoneIdcHandler
    : IRequestHandler<CountryCodePhoneIdcRequest, List<CountryCodePhoneIdcItem>>
{
    public Task<List<CountryCodePhoneIdcItem>> Handle(CountryCodePhoneIdcRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!     
        return Task.FromResult(new List<CountryCodePhoneIdcItem>
        {
            new CountryCodePhoneIdcItem() { Id = "AD+376", Name = "AD", Idc = "+376",  IsValid = true },
            new CountryCodePhoneIdcItem() { Id = "AE+971", Name = "AE", Idc = "+971",  IsValid = true },
            new CountryCodePhoneIdcItem() { Id = "AF+93", Name = "AF", Idc = "+93",  IsValid = true },
            new CountryCodePhoneIdcItem() { Id = "AG+1268", Name = "AG", Idc = "+1268",  IsValid = true },
            new CountryCodePhoneIdcItem() { Id = "AI+1264", Name = "AI", Idc = "+1264",  IsValid = true },
            new CountryCodePhoneIdcItem() { Id = "CZ+420", Name = "CZ", Idc = "+420",  IsValid = true },
        });
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<CountryCodePhoneIdcHandler> _logger;

    public CountryCodePhoneIdcHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<CountryCodePhoneIdcHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}