﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerOnSAIdentity", Schema = "dbo")]
internal class CustomerOnSAIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAIdentityId { get; set; }
    
    public int CustomerOnSAId { get; set; }
    
    public CIS.Foms.Enums.IdentitySchemes IdentityScheme { get; set; }
    
    public int Id { get; set; }

    public virtual CustomerOnSA Customer { get; set; } = null!;
}