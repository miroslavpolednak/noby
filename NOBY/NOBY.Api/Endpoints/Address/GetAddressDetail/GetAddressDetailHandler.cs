using System.Globalization;
using DomainServices.CodebookService.Clients;
using ExternalServices.AddressWhisperer.V1;
using ExternalServices.RuianAddress.V1;

namespace NOBY.Api.Endpoints.Address.GetAddressDetail;

internal sealed class GetAddressDetailHandler
    : IRequestHandler<GetAddressDetailRequest, GetAddressDetailResponse>
{
    private readonly IAddressWhispererClient _addressWhisperer;
    private readonly IRuianAddressClient _ruianAddress;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<GetAddressDetailHandler> _logger;

    public GetAddressDetailHandler(IAddressWhispererClient addressWhisperer, IRuianAddressClient ruianAddress, ICodebookServiceClient codebookService, ILogger<GetAddressDetailHandler> logger)
    {
        _addressWhisperer = addressWhisperer;
        _ruianAddress = ruianAddress;
        _codebookService = codebookService;
        _logger = logger;
    }

    public async Task<GetAddressDetailResponse> Handle(GetAddressDetailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.CountryId == IRuianAddressClient.AllowedCountryId)
                return await GetRuianAddressDetail(request.AddressId, cancellationToken);

            return await GetAddressWhispererDetail(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAddressDetail external service call failed: {Message}", ex.Message);
            throw new NobyValidationException(90020);
        }
    }

    private async Task<GetAddressDetailResponse> GetRuianAddressDetail(string addressId, CancellationToken cancellationToken)
    {
        var detail = await _ruianAddress.GetAddressDetail(long.Parse(addressId, CultureInfo.InvariantCulture), cancellationToken);

        return new GetAddressDetailResponse
        {
            Street = detail.StreetName,
            StreetNumber = $"{detail.OrientationNumber}{detail.OrientationNumberEndChar}",
            HouseNumber = detail.HouseNumber?.ToString(CultureInfo.InvariantCulture),
            Postcode = detail.ZipCode.ToString(CultureInfo.InvariantCulture),
            City = detail.TownName,
            CityDistrict = detail.TownPartName,
            PragueDistrict = detail.MopName,
            AddressPointId = detail.Id.ToString(CultureInfo.InvariantCulture),
            KatuzId = (int?)detail.TerritoryId,
            KatuzTitle = detail.TerritoryName,
            CountryId = IRuianAddressClient.AllowedCountryId
        };
    }

    private async Task<GetAddressDetailResponse> GetAddressWhispererDetail(GetAddressDetailRequest request, CancellationToken cancellationToken)
    {
        // zeme z ciselniku
        string country = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.Id == request.CountryId)?.ShortName
                         ?? throw new CisValidationException($"Country #{request.CountryId} not found");


        var result = await _addressWhisperer.GetAddressDetail(request.SessionId, request.AddressId, request.Title, country, cancellationToken);

        return result is null ? new GetAddressDetailResponse() : new GetAddressDetailResponse
        {
            StreetNumber = result.StreetNumber,
            City = result.City,
            CountryId = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.ShortName == result.Country)?.Id,
            CityDistrict = result.CityDistrict,
            HouseNumber = result.HouseNumber,
            Street = result.Street,
            PragueDistrict = result.PragueDistrict,
            Postcode = result.Postcode,
            EvidenceNumber = result.EvidenceNumber,
            DeliveryDetails = result.DeliveryDetails,
            AddressPointId = result.AddressPointId
        };
    }
}
