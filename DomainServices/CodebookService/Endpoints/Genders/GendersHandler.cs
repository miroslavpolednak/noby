using DomainServices.CodebookService.Contracts.Endpoints.Genders;

namespace DomainServices.CodebookService.Endpoints.Genders;

public class GendersHandler
    : IRequestHandler<GendersRequest, List<GenderItem>>
{
    public Task<List<GenderItem>> Handle(GendersRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Foms.Enums.Genders>()
            .Select(t => new GenderItem
            {
                Id = (int)t,
                Value = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                RDMCode = t == CIS.Foms.Enums.Genders.Female ? "F" : "M"
            })
            .ToList();

        return Task.FromResult(values);
    }
}
