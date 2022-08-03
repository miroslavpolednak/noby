using System.ComponentModel.DataAnnotations;
using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacities;

namespace DomainServices.CodebookService.Endpoints.LegalCapacities;

internal class LegalCapacitiesHandler
    : IRequestHandler<LegalCapacitiesRequest, List<LegalCapacityItem>>
{

    public Task<List<LegalCapacityItem>> Handle(LegalCapacitiesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.LegalCapacities>()
            .Select(t => new LegalCapacityItem()
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
