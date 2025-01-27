﻿namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest;

internal class Address
{
    private readonly GrpcAddress _address;

    public Address(GrpcAddress address)
    {
        _address = address;
    }

    public int? AddressTypeId => _address.AddressTypeId;

    public string Street =>
        new[] { _address.Street, _address.CityDistrict, _address.City }
            .FirstOrDefault(adr => !string.IsNullOrWhiteSpace(adr), string.Empty);

    public string HouseNumber => _address.HouseNumber;

    public string StreetNumber => _address.StreetNumber;

    public string EvidenceNumber => _address.EvidenceNumber;

    public string DeliveryDetails => _address.DeliveryDetails;

    public string? Postcode => string.IsNullOrWhiteSpace(_address.Postcode) ? null : _address.Postcode.Replace(" ", "");

    public string City => _address.City;

    public int? CountryId => _address.CountryId;

    public string CityDistrict => _address.CityDistrict;

    public string PragueDistrict => _address.PragueDistrict;

    public string CountrySubdivision => _address.CountrySubdivision;

    public string AddressPointId => _address.AddressPointId;
}