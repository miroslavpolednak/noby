using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Uver", Schema = "dbo")]
internal class Loan
{
    //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; set; }

    public Int64? PartnerId { get; set; }

	public int? KodProduktyUv { get; set; }

    public string? CisloSmlouvy { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? MesicniSplatka { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? VyseUveru { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? RadnaSazba { get; set; }

    public Int16? DelkaFixaceUrokoveSazby { get; set; }

    public int? AkceUveruId { get; set; }

    public int DruhUveru { get; set; }

    public int? UcelUveruId { get; set; }

    public DateTime? PredpDatum1Cerpani { get; set; }

    public DateTime? DatumPodpisuPrvniZadosti { get; set; }

    public Int64? PoradceId { get; set; }

    public Int16 TypUveru { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? ZbyvaCerpat { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? ZustatekCelkem { get; set; }

	public DateTime? DatumKonceCerpani { get; set; }

	public DateTime? DatumUzavreniSmlouvy { get; set; }

	public DateTime? DatumFixaceUrokoveSazby { get; set; }

	public DateTime? PocatekSplaceni { get; set; }

    public DateTime? DatumPrvniVyplatyZUveru { get; set; }

	public DateTime? DatumPredpSplatnosti { get; set; }

	public DateTime? DatumZbytkoveSplCelkem { get; set; }

	public string? CisloUctu { get; set; }

	public string? PredcisliUctu { get; set; }

    public short? HuVypisFrekvence { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? Jistina { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? CelkovyDluhPoSplatnosti { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? PohledavkaPoplatkyPo { get; set; }

	public int? PocetBankovnichDniPoSpl { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? SazbaZProdleni { get; set; }

	public int? SplatkyDen { get; set; }

	public long? PobockaObsluhyId { get; set; }

	public string? InkasoPredcisli { get; set; }

    public string? InkasoCislo { get; set; }

    public string? InkasoBanka { get; set; }

	public short? HuVypisZodb { get; set; }
	public short? HuVypisTyp { get; set; }
	public string? VypisEmail1 { get; set; }
    public string? VypisEmail2 { get; set; }
}
