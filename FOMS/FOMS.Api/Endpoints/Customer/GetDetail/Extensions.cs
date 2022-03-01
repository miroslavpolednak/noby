﻿using DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.GetDetail.Dto;

namespace FOMS.Api.Endpoints.Customer.GetDetail;

public static class Extensions
{
    public static GetDetailResponse ToResponseDto(this CustomerResponse customer)
    {
        var model = new GetDetailResponse();
        
        model.NaturalPerson = new NaturalPersonModel()
        {
            FirstName = customer.NaturalPerson?.FirstName,
            LastName = customer.NaturalPerson?.LastName,
            DateOfBirth = customer.NaturalPerson?.DateOfBirth,
            MaritalStatusStateId = customer.NaturalPerson?.MaritalStatusStateId,
            BirthName = customer.NaturalPerson?.BirthName,
            BirthNumber = customer.NaturalPerson?.BirthNumber,
            PlaceOfBirth = customer.NaturalPerson?.PlaceOfBirth,
            Gender = (CIS.Foms.Enums.Genders)(customer.NaturalPerson?.GenderId ?? 0),
            BirthCountryId = customer.NaturalPerson?.BirthCountryId,
            CitizenshipCountriesId = customer.NaturalPerson?.CitizenshipCountriesId?.Select(t => t).ToList()
        };

        model.IdentificationDocument = new IdentificationDocumentModel()
        {
            IssuingCountryId = customer.IdentificationDocument?.IssuingCountryId,
            IdentificationDocumentTypeId = customer.IdentificationDocument?.IdentificationDocumentTypeId,
            IssuedBy = customer.IdentificationDocument?.IssuedBy,
            IssuedOn = customer.IdentificationDocument?.IssuedOn,
            RegisterPlace = customer.IdentificationDocument?.RegisterPlace,
            ValidTo = customer.IdentificationDocument?.ValidTo
        };

        model.Contacts = customer.Contacts?
            .Select(t => new ContactModel()
            {
                ContactTypeId = t.ContactTypeId,
                Value = t.Value,
                IsPrimary = t.IsPrimary
            }).ToList();

        model.Addresses = customer.Addresses?
            .Select(t => new AddressModel()
            {
                Street = t.Street,
                Postcode = t.Postcode,
                IsPrimary = t.IsPrimary,
                AddressTypeId = t.AddressTypeId,
                LandRegistryNumber = t.LandRegistryNumber,
                BuildingIdentificationNumber = t.BuildingIdentificationNumber
            }).ToList();
        
        return model;
    }
}