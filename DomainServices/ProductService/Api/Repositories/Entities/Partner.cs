using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Repositories.Entities;

[Table("Partner", Schema = "dbo")]
internal class Partner
{
   
    public Int64 Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Jmeno { get; set; }

    public string Prijmeni { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public string? KBPartyId { get; set; }

}