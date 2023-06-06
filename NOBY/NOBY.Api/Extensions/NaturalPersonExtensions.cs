namespace NOBY.Api.Extensions;

internal static class NaturalPersonExtensions
{
    public static void FillResponseDto(this DomainServices.CustomerService.Contracts.NaturalPerson person, NOBY.Dto.BaseNaturalPerson transformedModel)
    {
        transformedModel.FirstName = person.FirstName;
        transformedModel.LastName = person.LastName;
        transformedModel.DateOfBirth = person.DateOfBirth;
        transformedModel.DegreeAfterId = person.DegreeAfterId;
        transformedModel.DegreeBeforeId = person.DegreeBeforeId;
        transformedModel.MaritalStatusId = person.MaritalStatusStateId;
        transformedModel.BirthName = person.BirthName;
        transformedModel.BirthNumber = person.BirthNumber;
        transformedModel.PlaceOfBirth = person.PlaceOfBirth;
        transformedModel.Gender = (CIS.Foms.Enums.Genders)person.GenderId;
        transformedModel.BirthCountryId = person.BirthCountryId;
        transformedModel.CitizenshipCountriesId = person.CitizenshipCountriesId?.Select(t => t).ToList();
    }
}
