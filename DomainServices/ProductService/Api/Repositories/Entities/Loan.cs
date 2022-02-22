using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Repositories.Entities;

[Table("Uver", Schema = "dbo")]
internal class Loan
{
    //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; set; }

    public Int64? PartnerId { get; set; }

    public string? CisloSmlouvy { get; set; }

    public decimal? MesicniSplatka { get; set; }

    public decimal? VyseUveru { get; set; }

    public decimal? RadnaSazba { get; set; }

    public Int16? DelkaFixaceUrokoveSazby { get; set; }

    public int? AkceUveruId { get; set; }

    public int DruhUveru { get; set; }

    public int? UcelUveruId { get; set; }

    public DateTime? PredpDatum1Cerpani { get; set; }

    public DateTime? DatumPodpisuPrvniZadosti { get; set; }

    public Int64? PoradceId { get; set; }

    public Int16 TypUveru { get; set; }

}