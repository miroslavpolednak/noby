using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.ExternalServices.Address.V2;
using DomainServices.CustomerService.ExternalServices.Address.V2.Contracts;

namespace DomainServices.CustomerService.Api.Endpoints.FormatAddress;

internal class FormatAddressHandler : IRequestHandler<FormatAddressRequest, FormatAddressResponse>
{
    private readonly ICustomerAddressServiceClient _addressService;
    private readonly ICodebookServiceClient _codebookService;

    public FormatAddressHandler(ICustomerAddressServiceClient addressService, ICodebookServiceClient codebookService)
    {
        _addressService = addressService;
        _codebookService = codebookService;
    }

    public async Task<FormatAddressResponse> Handle(FormatAddressRequest request, CancellationToken cancellationToken)
    {
        var singleLineAddress = await _addressService.FormatAddress(await ParseComponentAddress(request.Address, cancellationToken), cancellationToken);

        if (string.IsNullOrWhiteSpace(singleLineAddress))
        {
            if (string.IsNullOrWhiteSpace(request.Address.Street))
                singleLineAddress = await FormatAddressWithoutStreet(request.Address, cancellationToken);
            else
                singleLineAddress = await FormatAddressWithStreet(request.Address, cancellationToken);
        }

        return new FormatAddressResponse
        {
            SingleLineAddress = singleLineAddress
        };
    }

    private async Task<string> CountryCode(int? countryId, CancellationToken cancellationToken)
    {
        var countries = await _codebookService.Countries(cancellationToken);

        return countries.First(c => c.Id == countryId).ShortName;
    }

    private async Task<ComponentAddressPoint> ParseComponentAddress(GrpcAddress address, CancellationToken cancellationToken)
    {
        return new ComponentAddressPoint
        {
            Street = address.Street.ToCMString(),
            StreetNumber = address.StreetNumber.ToCMString(),
            HouseNumber = address.HouseNumber.ToCMString(),
            EvidenceNumber = address.EvidenceNumber.ToCMString(),
            City = address.City,
            CityDistrict = address.CityDistrict.ToCMString(),
            PragueDistrict = address.PragueDistrict.ToCMString(),
            DeliveryDetails = address.DeliveryDetails.ToCMString(),
            PostCode = address.Postcode.ToCMString(),
            CountryCode = await CountryCode(address.CountryId, cancellationToken),
            CountrySubdivision = address.CountrySubdivision.ToCMString(),
            AddressPointId = address.AddressPointId.ToCMString()
        };
    }

    private async Task<string> FormatAddressWithStreet(GrpcAddress address, CancellationToken cancellationToken)
    {
        var addressValues = RemoveEmptyAddressValues($"{address.Street} {FormatHouseNumber(address)}", 
                                                     address.City,
                                                     address.Postcode, 
                                                     await CountryCode(address.CountryId, cancellationToken));

        return string.Join(", ", addressValues);
    }

    private async Task<string> FormatAddressWithoutStreet(GrpcAddress address, CancellationToken cancellationToken)
    {
        var addressValues = RemoveEmptyAddressValues($"{address.CityDistrict} {FormatHouseNumber(address)}", 
                                                     address.City, 
                                                     address.Postcode, 
                                                     await CountryCode(address.CountryId, cancellationToken));

        return string.Join(", ", addressValues);
    }

    private static string FormatHouseNumber(GrpcAddress address)
    {
        if (!string.IsNullOrWhiteSpace(address.EvidenceNumber))
            return $"ev.č. {address.EvidenceNumber}";

        return string.Join("/", RemoveEmptyAddressValues(address.HouseNumber, address.StreetNumber));
    }

    private static IEnumerable<string> RemoveEmptyAddressValues(params string[] addressValues)
    {
        return addressValues.Where(str => !string.IsNullOrWhiteSpace(str));
    }
}