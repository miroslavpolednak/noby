using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

namespace DomainServices.CodebookService.Endpoints.HouseholdTypes;

internal class HouseholdTypesHandler
    : IRequestHandler<HouseholdTypesRequest, List<HouseholdTypeItem>>
{
    public Task<List<HouseholdTypeItem>> Handle(HouseholdTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.HouseholdTypes>()
            .Select(t => new HouseholdTypeItem()
            {
                Id = (int)t,
                Value = t,
                RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}