using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskCategories;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskCategories;

public class WorkflowTaskCategoriesHandler
    : IRequestHandler<WorkflowTaskCategoriesRequest, List<GenericCodebookItem>>
{
    public Task<List<GenericCodebookItem>> Handle(WorkflowTaskCategoriesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.WorkflowTaskCategory>()
            .Select(t => new GenericCodebookItem
            {
                Id = (int)t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
            })
            .ToList();

        return Task.FromResult(values);
    }
}
