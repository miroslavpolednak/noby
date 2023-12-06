IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'DDS' )
BEGIN
EXEC sp_executesql N'CREATE SCHEMA DDS'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[RealEstateValudationData]') AND type in (N'U'))
ALTER TABLE [DDS].[RealEstateValudationData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[RealEstateValudationData]
GO
DROP TABLE IF EXISTS [DDS].[RealEstateValudationDataHistory]
GO
CREATE TABLE [DDS].[RealEstateValudationData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_RealEstateValudationData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[RealEstateValudationDataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[RealEstateValudationData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[RealEstateValudationData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET IDENTITY_INSERT [DDS].[RealEstateValudationData] ON;
GO

INSERT INTO [DDS].[RealEstateValudationData]
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.RealEstateValuationId, A.RealEstateValuationId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{'+(case when len(A.LoanPurposeDetails)>0 then SUBSTRING(A.LoanPurposeDetails, 2, len(A.LoanPurposeDetails)-2) else '' end)+(case when len(A.Documents)>0 then (case when len(A.LoanPurposeDetails)>0 then ',' else '' end)+'"Documents":['+A.Documents+']' else '' end)+'}'
from RealEstateValuation A
where A.SpecificDetail is null
GO

INSERT INTO [DDS].[RealEstateValudationData]
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.RealEstateValuationId, A.RealEstateValuationId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"Parcel":{'+SUBSTRING(A.SpecificDetail, 2, len(A.SpecificDetail)-2)+'}'+(case when len(A.LoanPurposeDetails)>0 then ','+SUBSTRING(A.LoanPurposeDetails, 2, len(A.LoanPurposeDetails)-2) else '' end)+(case when len(A.Documents)>0 then ',"Documents":['+A.Documents+']' else '' end)+'}'
from RealEstateValuation A
where A.SpecificDetail like '{"ParcelNumbers%';
GO

INSERT INTO [DDS].[RealEstateValudationData]
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.RealEstateValuationId, A.RealEstateValuationId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"HouseAndFlat":{'+SUBSTRING(A.SpecificDetail, 2, len(A.SpecificDetail)-2)+'}'+(case when len(A.LoanPurposeDetails)>0 then ','+SUBSTRING(A.LoanPurposeDetails, 2, len(A.LoanPurposeDetails)-2) else '' end)+(case when len(A.Documents)>0 then ',"Documents":['+A.Documents+']' else '' end)+'}'
from RealEstateValuation A
where A.SpecificDetail like '{"PoorCondition%';
GO

SET IDENTITY_INSERT [DDS].[RealEstateValudationData] OFF;
GO

DROP TABLE IF EXISTS RealEstateValuation_backup;
GO
SELECT * INTO RealEstateValuation_backup FROM RealEstateValuation;
GO

ALTER TABLE [dbo].RealEstateValuation SET ( SYSTEM_VERSIONING = OFF  )
GO
ALTER TABLE [dbo].RealEstateValuation DROP COLUMN SpecificDetail;
GO
ALTER TABLE [dbo].RealEstateValuation DROP COLUMN SpecificDetailBin;
GO
ALTER TABLE [dbo].RealEstateValuation DROP COLUMN LoanPurposeDetails;
GO
ALTER TABLE [dbo].RealEstateValuation DROP COLUMN LoanPurposeDetailsBin;
GO
ALTER TABLE [dbo].RealEstateValuation DROP COLUMN Documents;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN SpecificDetail;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN SpecificDetailBin;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN LoanPurposeDetails;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN LoanPurposeDetailsBin;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN Documents;
GO
ALTER TABLE [dbo].RealEstateValuation SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationHistory])  );
GO
