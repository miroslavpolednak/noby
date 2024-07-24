namespace NOBY.ApiContracts;

[RollbackDescription("- volá CustomerOnSAService.UpdateCustomer() se snapshotem instance customera ze začátku requestu")]
public partial class CustomerIdentifyByIdentityRequest : IRequest<Unit>, CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable
{
    
    [JsonIgnore]
    public int CustomerOnSAId;

    public CustomerIdentifyByIdentityRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}