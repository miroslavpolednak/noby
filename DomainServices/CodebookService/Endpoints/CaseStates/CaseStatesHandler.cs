using DomainServices.CodebookService.Contracts.Endpoints.CaseStates;

namespace DomainServices.CodebookService.Endpoints.CaseStates;

public class CaseStatesHandler
    : IRequestHandler<CaseStatesRequest, List<CaseStateItem>>
{
    public Task<List<CaseStateItem>> Handle(CaseStatesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Core.Enums.CaseStates>()
            .Select(t => new CaseStateItem
            {
                Id = (int)t,
                Value = t,
                Name = t.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description,
                IsDefault = t == CIS.Core.Enums.CaseStates.InProcess
            })
            .ToList();

        return Task.FromResult(values);
    }
}
