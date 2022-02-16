using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;
using System.Linq;

namespace DomainServices.ProductService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class KonsDbRepository
    : DapperBaseRepository<NobyDbRepository>
{
    private const string _basicDetailSelect = "CisloSmlouvy, CilovaCastka, ZadaStatniPremii, StavUctuCelkem, UrokovaSazbaSporeni, DatumUzavreniSmlouvy";

    public KonsDbRepository(ILogger<NobyDbRepository> logger, IConnectionProvider<KonsDbRepository> factory)
        : base(logger, factory) { }

    public async Task<Dto.HousingSavingsBasicDetailModel> GetSavingsBasicDetail(long productInstanceId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.HousingSavingsBasicDetailModel>("SELECT " + _basicDetailSelect + " FROM dbo.Sporeni WHERE Id=@id", new { id = productInstanceId }));
    }

    public async Task<Dto.HousingSavingsFullDetailModel> GetSavingsFullDetail(long productInstanceId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.HousingSavingsFullDetailModel>("SELECT " + _basicDetailSelect + ", MesicniSplatka FROM dbo.Sporeni WHERE Id=@id", new { id = productInstanceId }));
    }

    public async Task<Dto.ProductInstanceListModel?> GetSavingsListItem(long caseId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.ProductInstanceListModel>("SELECT TOP 1 ProductInstanceId, ProductInstanceTypeId 'ProductInstanceTypeId', State FROM dbo.Sporeni WHERE Id=@id", new { id = caseId }));
    }

    //TODO jak najit uvery k SS podle CaseId?
    public async Task<List<Dto.ProductInstanceListModel>> GetSavingsLoanListItems(long caseId)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.ProductInstanceListModel>("SELECT ProductInstanceId, ProductInstanceTypeId 'ProductInstanceTypeId', State FROM dbo.Sporeni WHERE SporeniId=@id", new { id = caseId })).ToList());
    }
}


/*
 

  "partnerId": 1,											[PartnerId] [bigint] NULL,
  "loanContractNumber": "KB272",							[CisloSmlouvy] [varchar](10) NULL,
  "monthlyInstallment": 10000,								[MesicniSplatka] [decimal](16, 4) NULL,
  "loanAmount": 5000000,									[VyseUveru] [decimal](16, 4) NULL,
  "interestRate": 3.0,										[RadnaSazba] [decimal](16, 4) NULL,
  "fixationPeriod": 5,										[DelkaFixaceUrokoveSazby] [smallint] NULL,
  "loanType": "KBMortgage",
  "loanEventCode": 11,										[AkceUveruId] [int] NULL,
  "loanKind": 1,											[DruhUveru] [int] NOT NULL,
  "loanPurposeId": 2,										[UcelUveruId] [int] NULL,
  "expected1stDrawDate": "2022-10-10T12:34:07.476Z",		[PredpDatum1Cerpani] [datetime] NULL,
  "firstRequestSignDate": "2022-03-03T12:34:07.476Z",		[DatumPodpisuPrvniZadosti] [date] NULL,
  "consultantId": 9,										[PoradceId] [bigint] NULL,


CREATE TABLE [dbo].[Uver] (
	[Id] [bigint] NOT NULL,
	[CisloSmlouvy] [varchar](10) NULL,
	[SporeniId] [bigint] NULL,
	[ZbyvaCerpat] [decimal](16, 4) NULL,
	[DatumPosledniVyplatyZUveru] [datetime] NULL,
	[DatumUzavreniSmlouvy] [datetime] NULL,
	[DatumSchvaleniBankou] [datetime] NULL,
	[PartnerId] [bigint] NULL,
	[DatumOtevreniUctu] [datetime] NULL,
	[DatumZruseniVyporadani] [datetime] NULL,
	[JizVycerpanaCastka] [decimal](16, 4) NULL,
	[DatumFixaceUrokoveSazby] [datetime] NULL,
	[MesicniSplatka] [decimal](16, 4) NULL,
	[VyseUveru] [decimal](16, 4) NULL,
	[RadnaSazba] [decimal](16, 4) NULL,
	[DatumZavedeniDoSystemu] [datetime] NULL,
	[PoradceId] [bigint] NULL,
	[PoradceOrigId] [bigint] NULL,
	[DelkaFixaceUrokoveSazby] [smallint] NULL,
	[TypUveru] [smallint] NOT NULL,
	[ZustatekCelkem] [decimal](16, 4) NULL,
	[AkceUveruId] [int] NULL,
	[DruhUveru] [int] NOT NULL,
	[PredpDatum1Cerpani] [datetime] NULL,
	[ZahajitCerpaniDo] [datetime] NULL,
	[DatumSkutecnehoSplaceni] [datetime] NULL,
	[UcelUveruId] [int] NULL,
	[CisloUctu] [varchar](10) NULL,
	[PredcisliUctu] [varchar](6) NULL,
	[DatumPrvniVyplatyZUveru] [datetime] NULL,
	[DatumPredpSplatnosti] [datetime] NULL,
	[DatumZbytkoveSplatnosti] [datetime] NULL,
	[DatumZbytkoveSplCelkem] [datetime] NULL,
	[DatumKonceCerpani] [datetime] NULL,
	[DatumDosporovaniOd] [datetime] NULL,
	[MesicniDosporovani] [decimal](16, 4) NULL,
	[CerpaniUveruDo] [date] NULL,
	[CisloSporicihoUctu] [varchar](10) NULL,
	[DatumPodpisuPrvniZadosti] [date] NULL,
	[DatumSkenovaniZadosti] [date] NULL,
	[DatumZapisu] [datetime2](7) NOT NULL,
	[PodstavNavrhu] [smallint] NOT NULL,
	[PodstavNavrhuNazov] [nvarchar](50) NULL,
	[StavNavrhu] [smallint] NOT NULL,
	[StavNavrhuNazov] [nvarchar](50) NULL,
	[ZmNavrhId] [bigint] NULL,
	[DatumOdpisu] [date] NULL,
	[DatumZesplatneni] [date] NULL,
	[DuvodVyporadaniId] [int] NULL,
	[DatumPosledniMilnik] [date] NULL,
	[OnlineCerpaniMpHome] [bit] NOT NULL,
	[EtlChecksum] [int] NOT NULL,
	[EtlDataSource] [tinyint] NOT NULL,
	[EtlDataSourceSpec] [tinyint] NOT NULL,
	[EtlModified] [datetime2](7) NULL,
	[EtlRunId] [int] NOT NULL,
	[TimeStamp] [timestamp] NULL,
	[PredpocUrokyRadny] [decimal](16, 4) NULL,
	[MpHomeIsActive] [bit] NULL,
	[OcekavanaSplatka] [decimal](18, 2) NULL,
	[OcekavanaSplatkaDosporovani] [decimal](18, 2) NULL,
	[TrestanoPredplaceno] [decimal](18, 2) NULL,
	[NedoplatekDosporovani] [decimal](18, 2) NULL,
 */