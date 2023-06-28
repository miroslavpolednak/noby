using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("VztahUver", Schema = "dbo")]
internal class Relationship
{
   
    public Int64 Id { get; set; }

    public Int64 UverId { get; set; }

    public Int64 PartnerId { get; set; }

    public int VztahId { get; set; }

    public bool PreverenaVazba { get; set; }

    public DateTime? PlatnostOd { get; set; }

    public virtual Partner Partner { get; set; } = null!;

    public bool? Zmocnenec { get; set; }
}