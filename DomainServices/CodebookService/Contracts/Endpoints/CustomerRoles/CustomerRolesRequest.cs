namespace DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;

[DataContract]
public class CustomerRolesRequest : IRequest<List<CustomerRoleItem>>
{
}