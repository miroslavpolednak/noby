using __Data = DomainServices.HouseholdService.Api.Database.DocumentDataEntities;
using __Contract = DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataMappers;

internal sealed class IncomeEntrepreneurMapper
    : IDocumentDataMapper<__Data.IncomeEntrepreneur, __Contract.IncomeDataEntrepreneur>
{
    public __Data.IncomeEntrepreneur MapToDocumentData(__Contract.IncomeDataEntrepreneur source)
    {
        return new __Data.IncomeEntrepreneur
        {
            BirthNumber = source.BirthNumber,
            Cin = source.Cin,
            CountryOfResidenceId = source.CountryOfResidenceId
        };
    }

    public __Contract.IncomeDataEntrepreneur MapFromDocumentData(__Data.IncomeEntrepreneur data)
    {
        return new __Contract.IncomeDataEntrepreneur
        {
            BirthNumber = data.BirthNumber,
            Cin = data.Cin,
            CountryOfResidenceId = data.CountryOfResidenceId
        };
    }
}
