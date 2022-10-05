﻿using DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.GetDetail.Dto;
using Google.Protobuf.Collections;

namespace FOMS.Api.Endpoints.Customer.GetDetail;

internal static class Extensions
{
    public static NaturalPersonModel ToResponseDto(this NaturalPerson person)
        => new NaturalPersonModel()
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DegreeAfterId = person.DegreeAfterId,
            DegreeBeforeId = person.DegreeBeforeId,
            MaritalStatusId = person.MaritalStatusStateId,
            BirthName = person.BirthName,
            BirthNumber = person.BirthNumber,
            PlaceOfBirth = person.PlaceOfBirth,
            Gender = (CIS.Foms.Enums.Genders)person.GenderId,
            BirthCountryId = person.BirthCountryId,
            CitizenshipCountriesId = person.CitizenshipCountriesId?.Select(t => t).ToList(),
            IsBrSubscribed = person.IsBrSubscribed
        };

    public static IdentificationDocumentModel ToResponseDto(this IdentificationDocument document)
        => new IdentificationDocumentModel()
        {
            IssuingCountryId = document.IssuingCountryId,
            IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
            IssuedBy = document.IssuedBy,
            IssuedOn = document.IssuedOn,
            RegisterPlace = document.RegisterPlace,
            ValidTo = document.ValidTo,
            Number = document.Number
        };

    public static List<ContactModel>? ToResponseDto(this RepeatedField<Contact>? contacts)
        => contacts is null ? null : contacts.Select(t => new ContactModel()
        {
            ContactTypeId = t.ContactTypeId,
            Value = t.Value,
            IsPrimary = t.IsPrimary
        }).ToList();

    public static GetDetailResponse ToResponseDto(this CustomerDetailResponse customer)
        => new GetDetailResponse
        {
            NaturalPerson = customer.NaturalPerson?.ToResponseDto(),
            JuridicalPerson = null,
            IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
            Contacts = customer.Contacts?.ToResponseDto(),
            Addresses = customer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList()
        };
}