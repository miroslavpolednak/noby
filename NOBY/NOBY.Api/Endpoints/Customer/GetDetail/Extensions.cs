using DomainServices.CustomerService.Contracts;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.Customer.GetDetail.Dto;
using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.GetDetail;

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