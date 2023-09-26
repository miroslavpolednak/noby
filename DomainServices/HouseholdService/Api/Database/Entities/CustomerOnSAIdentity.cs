using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.HouseholdService.Api.Database.Entities;

[Table("CustomerOnSAIdentity", Schema = "dbo")]
internal sealed class CustomerOnSAIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAIdentityId { get; set; }

    public int CustomerOnSAId { get; set; }

    public SharedTypes.Enums.IdentitySchemes IdentityScheme { get; set; }

    public long IdentityId { get; set; }

    public CustomerOnSA Customer { get; set; }

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
        this.IdentityScheme = (SharedTypes.Enums.IdentitySchemes)(int)identity.IdentityScheme;
    }
}