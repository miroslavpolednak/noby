using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

namespace DomainServices.CodebookService.Endpoints.HouseholdTypes;

internal class HouseholdTypesHandler
    : IRequestHandler<HouseholdTypesRequest, List<HouseholdTypeItem>>
{
    public Task<List<HouseholdTypeItem>> Handle(HouseholdTypesRequest request, CancellationToken cancellationToken)
    {

        var documentTypeIds = new Dictionary<CIS.Foms.Enums.HouseholdTypes, int>
        {
            { CIS.Foms.Enums.HouseholdTypes.Main, 4 },
            { CIS.Foms.Enums.HouseholdTypes.Codebtor, 5 }
        };

        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.HouseholdTypes>()
            .Select(t => new HouseholdTypeItem()
            {
                Id = (int)t,
                EnumValue = t,
                RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                DocumentTypeId = documentTypeIds.ContainsKey(t) ? documentTypeIds[t] : null,
            })
            .ToList();

        return Task.FromResult(values);
    }
}