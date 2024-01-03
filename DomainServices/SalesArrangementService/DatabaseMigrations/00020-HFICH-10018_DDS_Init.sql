IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'DDS' )
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA DDS'
END

GO

CREATE TABLE [DDS].[SalesArrangementParameters](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[SalesArrangementParametersType] [tinyint] NOT NULL,
	[Data] [nvarchar](MAX) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL

	CONSTRAINT [PK_SalesArrangementParameters] PRIMARY KEY CLUSTERED ([DocumentDataStorageId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),
) ON [PRIMARY]
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[SalesArrangementParametersHistory]))

CREATE NONCLUSTERED INDEX [IX_EntityId] ON [DDS].[SalesArrangementParameters]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]