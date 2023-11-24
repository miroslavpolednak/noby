using __Data = DomainServices.HouseholdService.Api.Database.DocumentDataEntities;
using __Contract = DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataMappers;

internal sealed class IncomeOtherMapper
    : IDocumentDataMapper<__Data.IncomeOther, __Contract.IncomeDataOther>
{
    public __Data.IncomeOther MapToDocumentData(__Contract.IncomeDataOther source)
    {
        return new __Data.IncomeOther
        {
            IncomeOtherTypeId = source.IncomeOtherTypeId
        };
    }

    public __Contract.IncomeDataOther MapFromDocumentData(__Data.IncomeOther data)
    {
        return new __Contract.IncomeDataOther
        {
            IncomeOtherTypeId = data.IncomeOtherTypeId
        };
    }
}
