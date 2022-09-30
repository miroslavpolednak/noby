namespace FOMS.Api.Endpoints.Address.GetAddressDetail;

internal sealed class GetAddressDetailHandler
    : IRequestHandler<GetAddressDetailRequest, GetAddressDetailResponse>
{
    public async Task<GetAddressDetailResponse> Handle(GetAddressDetailRequest request, CancellationToken cancellationToken)
    {
        // zeme z ciselniku
        string country = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.Id == request.CountryId)?.ShortName
            ?? throw new CisValidationException($"Country #{request.CountryId} not found");

        var result = ServiceCallResult.ResolveAndThrowIfError<ExternalServices.AddressWhisperer.Shared.AddressDetail>(await _addressWhispererClient.GetAddressDetail(request.SessionId, request.AddressId, request.Title, country, cancellationToken));

        return new GetAddressDetailResponse
        {
            BuildingIdentificationNumber = result.BuildingIdentificationNumber,
            City = result.City,
            CountryId = (await _codebookService.Countries(cancellationToken)).FirstOrDefault(t => t.ShortName == result.Country)?.Id,
            CityDistrict = result.CityDistrict,
            LandRegistryNumber = result.LandRegistryNumber,
            Street = result.Street,
            PragueDistrict = result.PragueDistrict,
            Postcode = result.Postcode,
            EvidenceNumber = result.EvidenceNumber,
            DeliveryDetails = result.DeliveryDetails
        };
    }

    private ExternalServices.AddressWhisperer.V1.IAddressWhispererClient _addressWhispererClient;
    private DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public GetAddressDetailHandler(ExternalServices.AddressWhisperer.V1.IAddressWhispererClient addressWhispererClient, DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _addressWhispererClient = addressWhispererClient;
        _codebookService = codebookService;
    }
}
