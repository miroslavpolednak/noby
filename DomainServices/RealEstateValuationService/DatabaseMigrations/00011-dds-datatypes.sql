DROP TABLE IF EXISTS [DDS].[RealEstateValuationOrderData]
GO

DROP TABLE IF EXISTS [DDS].[RealEstateValuationOrderDataHistory]
GO

DROP TABLE IF EXISTS [dbo].[RealEstateValuationOld]
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[RealEstateValudationData]
GO

ALTER TABLE [DDS].[RealEstateValudationData] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE [DDS].[RealEstateValudationData] ADD _id int;
GO
UPDATE [DDS].[RealEstateValudationData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[RealEstateValudationData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.RealEstateValudationData._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[RealEstateValudationDataHistory] ADD _id int;
GO
UPDATE [DDS].[RealEstateValudationDataHistory] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[RealEstateValudationDataHistory] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.RealEstateValudationDataHistory._id', 'DocumentDataEntityId', 'COLUMN';
GO

ALTER TABLE [DDS].[RealEstateValudationData] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[RealEstateValudationDataHistory])  );
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[RealEstateValudationData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
