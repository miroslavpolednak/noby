DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[Income]
GO

ALTER TABLE [DDS].[Income] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE [DDS].[Income] ADD _id int;
GO
UPDATE [DDS].[Income] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[Income] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.Income._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[IncomeHistory] ADD _id int;
GO
UPDATE [DDS].[IncomeHistory] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[IncomeHistory] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.IncomeHistory._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[Income] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[IncomeHistory])  );
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[Income]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[Obligation]
GO

ALTER TABLE [DDS].[Obligation] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE [DDS].[Obligation] ADD _id int;
GO
UPDATE [DDS].[Obligation] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[Obligation] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.Obligation._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[ObligationHistory] ADD _id int;
GO
UPDATE [DDS].[ObligationHistory] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[ObligationHistory] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.ObligationHistory._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[Obligation] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[ObligationHistory])  );
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[Obligation]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[CustomerOnSAData]
GO

ALTER TABLE [DDS].[CustomerOnSAData] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE [DDS].[CustomerOnSAData] ADD _id int;
GO
UPDATE [DDS].[CustomerOnSAData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[CustomerOnSAData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.CustomerOnSAData._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[CustomerOnSADataHistory] ADD _id int;
GO
UPDATE [DDS].[CustomerOnSADataHistory] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[CustomerOnSADataHistory] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.CustomerOnSADataHistory._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[CustomerOnSAData] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[CustomerOnSADataHistory])  );
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[CustomerOnSAData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO