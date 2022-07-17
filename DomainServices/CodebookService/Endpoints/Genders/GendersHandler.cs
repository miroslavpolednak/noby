using DomainServices.CodebookService.Contracts.Endpoints.Genders;

namespace DomainServices.CodebookService.Endpoints.Genders;

public class GendersHandler
    : IRequestHandler<GendersRequest, List<GenderItem>>
{
    public Task<List<GenderItem>> Handle(GendersRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.Genders>()
            .Select(t => new GenderItem
            {
                Id = (int)t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                MpHomeCode = t,
                KonsDBCode = (int)t,
                KbCmCode = t == CIS.Foms.Enums.Genders.Female ? "F" : "M",
                StarBuildJsonCode = t == CIS.Foms.Enums.Genders.Female ? "Z" : "M"
            })
            .ToList();

        return Task.FromResult(values);
    }
}