using DomainServices.CodebookService.Contracts.Endpoints.Mandants;

namespace DomainServices.CodebookService.Endpoints.Mandants;

public class MandantTypesHandler
    : IRequestHandler<MandantsRequest, List<MandantsItem>>
{
    public Task<List<MandantsItem>> Handle(MandantsRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Core.Enums.Mandants>()
            .Where(t => t > 0)
            .Select(t => new MandantsItem
            {
                Code = t.ToString(),
                StarbuildId = (int)t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}
