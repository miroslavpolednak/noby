namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class GrpcAddress
{
    public static implicit operator Foms.Types.Address?(GrpcAddress? address)
    {
        if (address is null) return null;

        return new Foms.Types.Address
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
            EvidenceNumber = address.EvidenceNumber
        };
    }
    
    public static implicit operator GrpcAddress?(Foms.Types.Address? address)
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
            AddressPointId = address.AddressPointId ?? string.Empty
        };
    }
}
