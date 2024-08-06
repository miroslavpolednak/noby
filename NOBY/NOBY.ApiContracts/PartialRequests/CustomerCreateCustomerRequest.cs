namespace NOBY.ApiContracts;

[RollbackDescription("- volá CustomerOnSAService.UpdateCustomer() se snapshotem instance customera ze začátku requestu")]
public partial class CustomerCreateCustomerRequest : IRequest<CustomerCreateCustomerResponse>, CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public CustomerCreateCustomerRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}