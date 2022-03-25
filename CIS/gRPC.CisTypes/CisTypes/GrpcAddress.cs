namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class GrpcAddress
{
    public static implicit operator Foms.Types.Address?(GrpcAddress? address)
    {
        if (address is null) return null;
        return new Foms.Types.Address
        {
            Street = address.Street,
            City = address.City,
            BuildingIdentificationNumber = address.BuildingIdentificationNumber,
            CountryId = address.CountryId,
            LandRegistryNumber = address.LandRegistryNumber,
            Postcode = address.Postcode
        };
    }

    public static implicit operator GrpcAddress?(Foms.Types.Address? address)
    {
        if (address is null) return null;
        return new GrpcAddress
        {
            BuildingIdentificationNumber = address.BuildingIdentificationNumber,
            Street = address.Street,
            City = address.City,
            CountryId = address.CountryId,
            LandRegistryNumber = address.LandRegistryNumber,
            Postcode = address.Postcode
        };
    }
}
