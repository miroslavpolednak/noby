using DomainServices.CodebookService.Contracts.Endpoints.CaseStates;

namespace DomainServices.CodebookService.Endpoints.CaseStates;

public class CaseStatesHandler
    : IRequestHandler<CaseStatesRequest, List<CaseStateItem>>
{
    public Task<List<CaseStateItem>> Handle(CaseStatesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.CaseStates>()
            .Select(t => new CaseStateItem
            {
                Id = (int)t,
                Code = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
            })
            .ToList();

        return Task.FromResult(values);
    }
}
