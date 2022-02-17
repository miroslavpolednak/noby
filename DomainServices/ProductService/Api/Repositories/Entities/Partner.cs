using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Repositories.Entities;

[Table("Partner", Schema = "dbo")]
internal class Partner
{
   
    public Int64 Id { get; set; }

    public string Jmeno { get; set; }

    public string Prijmeni { get; set; }

}