using DomainServices.CodebookService.Clients;

namespace DomainServices.CustomerService.Api.Endpoints.v1.FormatAddress;

internal sealed class FormatAddressHandler(ICodebookServiceClient _codebookService)
    : IRequestHandler<FormatAddressRequest, FormatAddressResponse>
{
    public async Task<FormatAddressResponse> Handle(FormatAddressRequest request, CancellationToken cancellationToken)
    {
        var countryName = await CountryName(request.Address.CountryId!.Value, cancellationToken);

        return new FormatAddressResponse
        {
            SingleLineAddress = JoinAddressValues(CreateStreetPart(request.Address),
                                                  CreateCityNamePart(request.Address),
                                                  GetFormattedPostCode(request.Address.Postcode, request.Address.CountryId.Value),
                                                  countryName)
        };

        static string JoinAddressValues(params string[] values) => string.Join(", ", values.Where(str => !string.IsNullOrWhiteSpace(str)));
    }

    private async Task<string> CountryName(int countryId, CancellationToken cancellationToken)
    {
        var countries = await _codebookService.Countries(cancellationToken);

        return countries.First(c => c.Id == countryId).LongName;
    }

    private static string CreateStreetPart(SharedTypes.GrpcTypes.GrpcAddress address)
    {
        //PO BOX part in the future

        var streetName = new[] { address.Street, address.CityDistrict, address.City }.First(str => !string.IsNullOrWhiteSpace(str));
        var houseNumber = CreateHouseNumber(address);

        if (string.IsNullOrWhiteSpace(houseNumber))
            return streetName;

        return string.IsNullOrWhiteSpace(streetName) ? houseNumber : $"{streetName} {houseNumber}";
    }

    private static string CreateHouseNumber(SharedTypes.GrpcTypes.GrpcAddress address)
    {
        var houseNumber = string.IsNullOrWhiteSpace(address.HouseNumber) ? $"ev. č. {address.EvidenceNumber}" : address.HouseNumber;

        if (string.IsNullOrWhiteSpace(address.StreetNumber))
            return houseNumber;

        return string.IsNullOrWhiteSpace(houseNumber) ? address.StreetNumber : $"{houseNumber}/{address.StreetNumber}";
    }

    private static string CreateCityNamePart(SharedTypes.GrpcTypes.GrpcAddress address)
    {
        if (IsPrague(address))
        {
            if (string.IsNullOrWhiteSpace(address.CityDistrict) || address.City.Equals(address.CityDistrict, StringComparison.OrdinalIgnoreCase))
                return address.PragueDistrict;

            return $"{address.PragueDistrict} - {address.CityDistrict}";
        }

        if (string.IsNullOrWhiteSpace(address.Street))
            return address.City;

        if (!string.IsNullOrWhiteSpace(address.CityDistrict) && address.CityDistrict.StartsWith(address.City, StringComparison.OrdinalIgnoreCase))
            return address.CityDistrict;

        if (string.IsNullOrWhiteSpace(address.CityDistrict) || address.City.Equals(address.CityDistrict, StringComparison.OrdinalIgnoreCase))
            return address.City;

        return $"{address.City} - {address.CityDistrict}";
    }

    private static string GetFormattedPostCode(string? postCode, int countryId)
    {
        if (string.IsNullOrWhiteSpace(postCode))
            return string.Empty;

        if (!IsCzech(countryId) || postCode.Length < 5 || postCode[3] == ' ')
            return postCode;

        return $"PSČ {postCode.Replace(" ", "").Insert(3, " ")}";
    }

    private static bool IsPrague(SharedTypes.GrpcTypes.GrpcAddress address) =>
        IsCzech(address.CountryId!.Value) && address.City == "Praha" && !string.IsNullOrWhiteSpace(address.PragueDistrict);

    private static bool IsCzech(int countryId) => countryId == 16;
}