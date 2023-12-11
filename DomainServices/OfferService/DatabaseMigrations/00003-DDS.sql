IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'DDS' )
BEGIN
EXEC sp_executesql N'CREATE SCHEMA DDS'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[OfferData]') AND type in (N'U'))
ALTER TABLE [DDS].[OfferData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[OfferData]
GO
DROP TABLE IF EXISTS [DDS].[OfferDataHistory]
GO
CREATE TABLE [DDS].[OfferData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_OfferData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[OfferDataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[OfferData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[OfferData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[AdditionalSimulationResultsData]') AND type in (N'U'))
ALTER TABLE [DDS].[AdditionalSimulationResultsData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[AdditionalSimulationResultsData]
GO
DROP TABLE IF EXISTS [DDS].[AdditionalSimulationResultsDataHistory]
GO
CREATE TABLE [DDS].[AdditionalSimulationResultsData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_AdditionalSimulationResultsData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[AdditionalSimulationResultsDataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[AdditionalSimulationResultsData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[AdditionalSimulationResultsData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[CreditWorthinessSimpleData]') AND type in (N'U'))
ALTER TABLE [DDS].[CreditWorthinessSimpleData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[CreditWorthinessSimpleData]
GO
DROP TABLE IF EXISTS [DDS].[CreditWorthinessSimpleDataHistory]
GO
CREATE TABLE [DDS].[CreditWorthinessSimpleData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_CreditWorthinessSimpleData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[CreditWorthinessSimpleDataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[CreditWorthinessSimpleData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[CreditWorthinessSimpleData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


select --A.OfferId, 1, A.CreatedUserId, A.CreatedTime,
	A.basicparameters,
	'{'
	+'"BasicParameters":{"FinancialResourcesOwn":'+(case when B.FinancialResourcesOwn is null then 'null' else cast(B.FinancialResourcesOwn as varchar(20)) end)+',"FinancialResourcesOther":'+(case when B.FinancialResourcesOther is null then 'null' else cast(B.FinancialResourcesOther as varchar(20)) end)+',"StatementTypeId":'+cast(isnull(B.StatementTypeId,0) as varchar(20))
	+',"GuaranteeDateTo":'+(case when B.GuaranteeDateTo_y is null then 'null' else '"'+cast(B.GuaranteeDateTo_y as varchar(4))+'-'+FORMAT(B.GuaranteeDateTo_m, 'D2')+'-'+FORMAT(B.GuaranteeDateTo_d, 'D2')+'T00:00:00"' end)+'}'
	+'"SimulationInputs":{}'
	+'"SimulationOutputs":{}'
	+'}'
	from dbo.Offer A 
	inner join (select y.*, X.OfferId from dbo.Offer X
		CROSS APPLY OPENJSON (X.BasicParameters) 
		with (
			FinancialResourcesOwn int '$.FinancialResourcesOwn.Units', 
			FinancialResourcesOther int '$.FinancialResourcesOther.Units', 
			StatementTypeId int '$.StatementTypeId', 
			GuaranteeDateTo_d int '$.GuaranteeDateTo.Day',
			GuaranteeDateTo_m int '$.GuaranteeDateTo.Month',
			GuaranteeDateTo_y int '$.GuaranteeDateTo.Year'
		) as Y) as B ON B.OfferId=A.OfferId
	inner join (select y.*, X.OfferId from dbo.Offer X
		CROSS APPLY OPENJSON (X.BasicParameters) 
		with (
			FinancialResourcesOwn int '$.FinancialResourcesOwn.Units', 
			FinancialResourcesOther int '$.FinancialResourcesOther.Units', 
			StatementTypeId int '$.StatementTypeId', 
			GuaranteeDateTo_d int '$.GuaranteeDateTo.Day',
			GuaranteeDateTo_m int '$.GuaranteeDateTo.Month',
			GuaranteeDateTo_y int '$.GuaranteeDateTo.Year'
		) as Y) as C ON C.OfferId=A.OfferId


DROP TABLE IF EXISTS Offer_backup;
GO
SELECT * INTO Offer_backup FROM Offer;
GO

ALTER TABLE [dbo].[Offer] DROP COLUMN BasicParameters;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationInputs;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationResults;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN AdditionalSimulationResults;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN BasicParametersBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationInputsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationResultsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN AdditionalSimulationResultsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN CreditWorthinessSimpleInputs;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN CreditWorthinessSimpleInputsBin;
GO
