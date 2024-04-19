ALTER TABLE [DDS].MortgageAdditionalSimulationResultsData SET ( SYSTEM_VERSIONING = OFF  );
GO
ALTER TABLE [DDS].MortgageAdditionalSimulationResultsData DROP PERIOD FOR SYSTEM_TIME;
GO
DROP TABLE DDS.MortgageAdditionalSimulationResultsDataHistory
GO
ALTER TABLE [DDS].MortgageAdditionalSimulationResultsData DROP COLUMN ValidFrom;
GO
ALTER TABLE [DDS].MortgageAdditionalSimulationResultsData DROP COLUMN ValidTo;
GO

ALTER TABLE [DDS].MortgageCreditWorthinessSimpleData SET ( SYSTEM_VERSIONING = OFF  );
GO
ALTER TABLE [DDS].MortgageCreditWorthinessSimpleData DROP PERIOD FOR SYSTEM_TIME;
GO
DROP TABLE DDS.MortgageCreditWorthinessSimpleDataHistory
GO
ALTER TABLE [DDS].MortgageCreditWorthinessSimpleData DROP COLUMN ValidFrom;
GO
ALTER TABLE [DDS].MortgageCreditWorthinessSimpleData DROP COLUMN ValidTo;
GO

ALTER TABLE [DDS].MortgageOfferData SET ( SYSTEM_VERSIONING = OFF  );
GO
ALTER TABLE [DDS].MortgageOfferData DROP PERIOD FOR SYSTEM_TIME;
GO
DROP TABLE DDS.MortgageOfferDataHistory
GO
ALTER TABLE [DDS].MortgageOfferData DROP COLUMN ValidFrom;
GO
ALTER TABLE [DDS].MortgageOfferData DROP COLUMN ValidTo;
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageAdditionalSimulationResultsData]
GO

ALTER TABLE [DDS].[MortgageAdditionalSimulationResultsData] ADD _id int;
GO
UPDATE [DDS].[MortgageAdditionalSimulationResultsData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageAdditionalSimulationResultsData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageAdditionalSimulationResultsData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageAdditionalSimulationResultsData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageCreditWorthinessSimpleData]
GO

ALTER TABLE [DDS].[MortgageCreditWorthinessSimpleData] ADD _id int;
GO
UPDATE [DDS].[MortgageCreditWorthinessSimpleData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageCreditWorthinessSimpleData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageCreditWorthinessSimpleData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageCreditWorthinessSimpleData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageExtraPaymentData]
GO

ALTER TABLE [DDS].[MortgageExtraPaymentData] ADD _id int;
GO
UPDATE [DDS].[MortgageExtraPaymentData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageExtraPaymentData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageExtraPaymentData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageExtraPaymentData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageOfferData]
GO

ALTER TABLE [DDS].[MortgageOfferData] ADD _id int;
GO
UPDATE [DDS].[MortgageOfferData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageOfferData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageOfferData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageOfferData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageRefixationData]
GO

ALTER TABLE [DDS].[MortgageRefixationData] ADD _id int;
GO
UPDATE [DDS].[MortgageRefixationData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageRefixationData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageRefixationData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageRefixationData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[MortgageRetentionData]
GO

ALTER TABLE [DDS].[MortgageRetentionData] ADD _id int;
GO
UPDATE [DDS].[MortgageRetentionData] SET _id = CAST([DocumentDataEntityId] as int);
GO
ALTER TABLE [DDS].[MortgageRetentionData] DROP COLUMN [DocumentDataEntityId];
GO
EXEC sp_rename 'DDS.MortgageRetentionData._id', 'DocumentDataEntityId', 'COLUMN';
GO

CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[MortgageRetentionData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
