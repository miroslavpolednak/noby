using DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;

namespace DomainServices.CodebookService.Endpoints.CustomerRoles;

public class CustomerRolesHandler
    : IRequestHandler<CustomerRolesRequest, List<CustomerRoleItem>>
{
    public Task<List<CustomerRoleItem>> Handle(CustomerRolesRequest request, CancellationToken cancellationToken)
    {
        //TODO: pořešit uložení RdmCodes (načítat z SB číselníku / uložit do extension table / doplnit atribut obdobně jako [CisDefaultValue]?)
        var dicRdmCodesById = new Dictionary<int, string> {
            { 0, null },
            { 1, "A" }, 
            { 2, "S" },
            { 128, "R" },
        };

        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Foms.Enums.CustomerRoles>()
            .Select(t => new CustomerRoleItem()
            {
                Id = (int)t,
                RdmCode = dicRdmCodesById[(int)t],
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}