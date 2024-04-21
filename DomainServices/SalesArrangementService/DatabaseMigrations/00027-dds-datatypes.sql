ALTER TABLE [dbo].[SalesArrangementParameters] SET ( SYSTEM_VERSIONING = OFF  );
GO
DROP TABLE IF EXISTS [dbo].[SalesArrangementParameters]
GO
DROP TABLE IF EXISTS [dbo].[SalesArrangementParametersHistory]
GO

DROP INDEX IF EXISTS [IX_EntityId] ON [DDS].[SalesArrangementParameters]
GO

ALTER TABLE [DDS].[SalesArrangementParameters] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE [DDS].[SalesArrangementParameters] ADD _id int;
GO
UPDATE [DDS].[SalesArrangementParameters] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[SalesArrangementParameters] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.SalesArrangementParameters._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[SalesArrangementParametersHistory] ADD _id int;
GO
UPDATE [DDS].[SalesArrangementParametersHistory] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[SalesArrangementParametersHistory] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.SalesArrangementParametersHistory._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[SalesArrangementParameters] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[SalesArrangementParametersHistory])  );
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[SalesArrangementParameters]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
