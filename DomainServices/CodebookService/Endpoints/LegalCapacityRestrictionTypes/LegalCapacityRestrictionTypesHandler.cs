using System.ComponentModel.DataAnnotations;
using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacityRestrictionTypes;

namespace DomainServices.CodebookService.Endpoints.LegalCapacityRestrictionTypes;

internal class LegalCapacityRestrictionTypesHandler
    : IRequestHandler<LegalCapacityRestrictionTypesRequest, List<LegalCapacityRestrictionTypeItem>>
{

    public Task<List<LegalCapacityRestrictionTypeItem>> Handle(LegalCapacityRestrictionTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.LegalCapacityRestrictions>()
            .Select(t => new LegalCapacityRestrictionTypeItem()
            {
                Id = (int)t,
                Code = t,
                Name = t.GetAttribute<DisplayAttribute>()?.Name ?? String.Empty,
                Description = t.GetAttribute<DisplayAttribute>()?.ShortName ?? String.Empty,
            })
            .ToList();

        return Task.FromResult(values);
    }

}
