using SharedTypes.GrpcTypes;

namespace NOBY.ApiContracts;

public partial class SharedTypesAddress
{
    [JsonIgnore]
    public bool IsPrimary { get; set; }

    public static implicit operator SharedTypesAddress?(GrpcAddress? address)
    {
        if (address is null) return null;

        return new SharedTypesAddress
        {
            IsPrimary = address.IsPrimary,
            Street = address.Street,
            City = address.City,
            StreetNumber = address.StreetNumber,
            CountryId = address.CountryId,
            HouseNumber = address.HouseNumber,
            Postcode = address.Postcode,
            AddressTypeId = address.AddressTypeId,
            CityDistrict = address.CityDistrict,
            PragueDistrict = address.PragueDistrict,
            CountrySubdivision = address.CountrySubdivision,
            AddressPointId = address.AddressPointId,
            DeliveryDetails = address.DeliveryDetails,
            EvidenceNumber = address.EvidenceNumber,
            SingleLineAddressPoint = address.SingleLineAddressPoint,
            IsAddressConfirmed = address.IsAddressConfirmed
        };
    }

    public static implicit operator GrpcAddress?(SharedTypesAddress? address)
    {
        if (address is null) return null;

        return new GrpcAddress
        {
            IsPrimary = address.IsPrimary,
            DeliveryDetails = address.DeliveryDetails ?? "",
            EvidenceNumber = address.EvidenceNumber ?? "",
            StreetNumber = address.StreetNumber ?? "",
            Street = address.Street ?? "",
            City = address.City ?? "",
            CountryId = address.CountryId,
            HouseNumber = address.HouseNumber ?? "",
            Postcode = address.Postcode ?? "",
            AddressTypeId = address.AddressTypeId,
            CityDistrict = address.CityDistrict ?? string.Empty,
            PragueDistrict = address.PragueDistrict ?? string.Empty,
            CountrySubdivision = address.CountrySubdivision ?? string.Empty,
            AddressPointId = address.AddressPointId ?? string.Empty,
            SingleLineAddressPoint = address.SingleLineAddressPoint,
            IsAddressConfirmed = address.IsAddressConfirmed
        };
    }
}
