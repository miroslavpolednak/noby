using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.HouseholdService.Api.Repositories.Entities;

[Table("CustomerOnSAIdentity", Schema = "dbo")]
internal class CustomerOnSAIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAIdentityId { get; set; }
    
    public int CustomerOnSAId { get; set; }
    
    public CIS.Foms.Enums.IdentitySchemes IdentityScheme { get; set; }
    
    public long IdentityId { get; set; }
    
    public virtual CustomerOnSA Customer { get; set; }

#pragma warning disable CS8618
    public CustomerOnSAIdentity()
#pragma warning restore CS8618
    {
    }

#pragma warning disable CS8618
    public CustomerOnSAIdentity(CIS.Infrastructure.gRPC.CisTypes.Identity identity, int? customerOnSAId = default(int?))
#pragma warning restore CS8618
    {
        if (customerOnSAId.GetValueOrDefault() > 0)
            this.CustomerOnSAId = customerOnSAId!.Value;
        this.IdentityId = identity.IdentityId;
        this.IdentityScheme = (CIS.Foms.Enums.IdentitySchemes)(int)identity.IdentityScheme;
    }
}