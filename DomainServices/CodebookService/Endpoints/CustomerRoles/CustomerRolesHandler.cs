using DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;

namespace DomainServices.CodebookService.Endpoints.CustomerRoles;

public class CustomerRolesHandler
    : IRequestHandler<CustomerRolesRequest, List<CustomerRoleItem>>
{
    public Task<List<CustomerRoleItem>> Handle(CustomerRolesRequest request, CancellationToken cancellationToken)
    {
        //TODO: pořešit uložení RdmCodes (načítat z SB číselníku / uložit do extension table / doplnit atribut obdobně jako [CisDefaultValue]?)
        var extensionsById = new Dictionary<int, string?> {
            { 0, null },
            { 1, "Hlavní žadatel" }, 
            { 2, "Spoludlužník" },
            { 128, "Ručitel" },
        };

        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.CustomerRoles>()
            .Select(t => new CustomerRoleItem()
            {
                Id = (int)t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName,
                NameNoby = extensionsById[(int)t],
            })
            .ToList();

        return Task.FromResult(values);
    }
}