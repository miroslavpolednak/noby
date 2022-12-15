using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData.ProductRequest;

internal class Address
{
    private readonly GrpcAddress _address;

    public Address(GrpcAddress address)
    {
        _address = address;
    }

    public int? AddressTypeId => _address.AddressTypeId;

    public string Street => string.IsNullOrWhiteSpace(_address.Street) ? _address.City : _address.Street;

    public string HouseNumber => _address.HouseNumber;

    public string StreetNumber => _address.StreetNumber;

    public string EvidenceNumber => _address.EvidenceNumber;

    public string DeliveryDetails => _address.DeliveryDetails;

    public string? Postcode
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_address.Postcode))
                return null;

            var value = _address.Postcode.Replace(" ", "");

            return value.All(char.IsNumber) ? value : throw new InvalidOperationException($"PostCode value '{value}' isn't covertable to number.");
        }
    }

    public string City => _address.City;

    public int? CountryId => _address.CountryId;

    public string CityDistrict => _address.CityDistrict;

    public string PragueDistrict => _address.PragueDistrict;

    public string CountrySubdivision => _address.CountrySubdivision;

    public string AddressPointId => _address.AddressPointId;
}