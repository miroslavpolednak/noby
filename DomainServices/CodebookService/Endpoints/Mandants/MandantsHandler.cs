using DomainServices.CodebookService.Contracts.Endpoints.Mandants;

namespace DomainServices.CodebookService.Endpoints.Mandants;

public class MandantTypesHandler
    : IRequestHandler<MandantsRequest, List<MandantsItem>>
{
    public Task<List<MandantsItem>> Handle(MandantsRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.Mandants>()
            .Where(t => t > 0)
            .Select(t => new MandantsItem
            {
                Id = (int)t,
                Code = t.ToString(),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}
