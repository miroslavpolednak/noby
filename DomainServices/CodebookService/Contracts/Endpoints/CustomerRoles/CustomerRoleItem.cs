namespace DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;

[DataContract]
public class CustomerRoleItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string RdmCode { get; set; }
    
    [DataMember(Order = 3)]
    public string Name { get; set; }
}