using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Uver", Schema = "dbo")]
internal class Loan
{
    //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; set; }

    public Int64? PartnerId { get; set; }

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
}

/*
 * 
partnerId
id
CisloSmlouvy
VyseUveru
RadnaSazba
DelkaFixaceUrokoveSazby
TypUveru
MesicniSplatka
-
ZustatekCelkem
DatumKonceCerpani
-
-
-
"tabulka dbo.VztahUver

PartnerId"
"tabulka dbo.VztahUver

VztahId"
DatumUzavreniSmlouvy
DatumFixaceUrokoveSazby
ZbyvaCerpat
Jistina
DruhUveru
ZustatekCelkem
DatumPrvniVyplatyZUveru
DatumPredpSplatnosti DatumZbytkoveSplCelkem
CPM
PoradceId

US_DOHODNUTA
US_DOHODNUTA_DATUM_OD
PERIODA_FIXACE_DOHODNUTA


[Audit_Id][bigint] IDENTITY(1,1) NOT NULL,
	[Audit_Date_Change] [datetime]
NOT NULL,
	[Id] [bigint]
NOT NULL,
	[CisloSmlouvy] [varchar] (10) NULL,
	[SporeniId][bigint] NULL,
	[ZbyvaCerpat][decimal] (16, 4) NULL,
	[DatumPosledniVyplatyZUveru][datetime] NULL,
	[DatumUzavreniSmlouvy][datetime] NULL,
	[DatumSchvaleniBankou][datetime] NULL,
	[PartnerId][bigint] NULL,
	[DatumOtevreniUctu][datetime] NULL,
	[DatumZruseniVyporadani][datetime] NULL,
	[JizVycerpanaCastka][decimal] (16, 4) NULL,
	[DatumFixaceUrokoveSazby][datetime] NULL,
	[MesicniSplatka][decimal] (16, 4) NULL,
	[VyseUveru][decimal] (16, 4) NULL,
	[RadnaSazba][decimal] (16, 4) NULL,
	[DatumZavedeniDoSystemu][datetime] NULL,
	[PoradceId][bigint] NULL,
	[PoradceOrigId][bigint] NULL,
	[DelkaFixaceUrokoveSazby][smallint] NULL,
	[TypUveru]
[smallint]
NOT NULL,
	[ZustatekCelkem] [decimal] (16, 4) NULL,
	[AkceUveruId][int] NULL,
	[DruhUveru][int] NULL,
	[PredpDatum1Cerpani][datetime] NULL,
	[ZahajitCerpaniDo][datetime] NULL,
	[DatumSkutecnehoSplaceni][datetime] NULL,
	[UcelUveruId][int] NULL,
	[CisloUctu][varchar] (10) NULL,
	[PredcisliUctu][varchar] (6) NULL,
	[DatumPrvniVyplatyZUveru][datetime] NULL,
	[DatumPredpSplatnosti][datetime] NULL,
	[DatumZbytkoveSplatnosti][datetime] NULL,
	[DatumZbytkoveSplCelkem][datetime] NULL,
	[DatumKonceCerpani][datetime] NULL,
	[DatumDosporovaniOd][datetime] NULL,
	[MesicniDosporovani][decimal] (16, 4) NULL,
	[CerpaniUveruDo][date] NULL,
	[CisloSporicihoUctu][varchar] (10) NULL,
	[DatumPodpisuPrvniZadosti][date] NULL,
	[DatumSkenovaniZadosti][date] NULL,
	[DatumZapisu][datetime2] (7) NULL,
	[PodstavNavrhu][smallint] NULL,
	[PodstavNavrhuNazov][nvarchar] (50) NULL,
	[StavNavrhu][smallint] NULL,
	[StavNavrhuNazov][nvarchar] (50) NULL,
	[ZmNavrhId][bigint] NULL,
	[ZdrojDat][tinyint] NULL,
	[MpTransportId][int] NULL,
	[DatumOdpisu][date] NULL,
	[DatumZesplatneni][date] NULL,
	[DuvodVyporadaniId][int] NULL,
	[OnlineCerpaniMpHome][bit] NULL,
	[DatumPosledniMilnik][date] NULL,
	[EtlChecksum]
[int]
NOT NULL,
	[EtlDataSource] [tinyint]
NOT NULL,
	[EtlDataSourceSpec] [tinyint]
NOT NULL,
	[EtlModified] [datetime2] (7) NULL,
	[EtlRunId]
[int]
NOT NULL,
	[PredpocUrokyRadny] [decimal] (16, 4) NULL,
	[MpHomeIsActive][bit] NULL,
	[OcekavanaSplatka][decimal] (18, 2) NULL,
	[OcekavanaSplatkaDosporovani][decimal] (18, 2) NULL,
	[TrestanoPredplaceno][decimal] (18, 2) NULL,
	[NedoplatekDosporovani][decimal] (18, 2) NULL,
*/