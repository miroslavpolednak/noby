using DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;

namespace DomainServices.CodebookService.Endpoints.CustomerRoles;

public class CustomerRolesHandler
    : IRequestHandler<CustomerRolesRequest, List<CustomerRoleItem>>
{
    public Task<List<CustomerRoleItem>> Handle(CustomerRolesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Foms.Enums.CustomerRoles>()
            .Select(t => new CustomerRoleItem()
            {
                Id = (int)t,
                Value = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}