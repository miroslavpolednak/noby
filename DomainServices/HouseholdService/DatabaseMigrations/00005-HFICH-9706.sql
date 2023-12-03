IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[CustomerOnSAData]') AND type in (N'U'))
ALTER TABLE [DDS].[CustomerOnSAData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[CustomerOnSAData]
GO
DROP TABLE IF EXISTS [DDS].[CustomerOnSADataHistory]
GO
CREATE TABLE [DDS].[CustomerOnSAData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_CustomerOnSAData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[CustomerOnSADataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[CustomerOnSAData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[CustomerOnSAData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET IDENTITY_INSERT [DDS].[CustomerOnSAData] ON;
GO

INSERT INTO [DDS].CustomerOnSAData
(DocumentDataStorageId,[DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.CustomerOnSAId, A.CustomerOnSAId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"AdditionalData":{"IsAddressWhispererUsed":'+(case X1.IsAddressWhispererUsed when null then 'null' when 1 then 'true' else 'false' end)
+',"HasRelationshipWithKB":'+(case X1.HasRelationshipWithKB when null then 'null' when 1 then 'true' else 'false' end)
+',"HasRelationshipWithKBEmployee":'+(case X1.HasRelationshipWithKBEmployee when null then 'null' when 1 then 'true' else 'false' end)
+',"HasRelationshipWithCorporate":'+(case X1.HasRelationshipWithCorporate when null then 'null' when 1 then 'true' else 'false' end)
+',"IsPoliticallyExposed":'+(case X1.IsPoliticallyExposed when null then 'null' when 1 then 'true' else 'false' end)
+',"IsUSPerson":'+(case X1.IsUSPerson when null then 'null' when 1 then 'true' else 'false' end)
+',"LegalCapacity":{"RestrictionTypeId":'+isnull(cast(RestrictionTypeId as varchar(10)),'null')+',"RestrictionUntil":'+(case when RestrictionUntil_y is null then 'null' else '"'+cast(RestrictionUntil_y as varchar(4))+'-'+FORMAT(RestrictionUntil_m, 'D2')+'-'+FORMAT(RestrictionUntil_d, 'D2')+'T00:00:00"' end)+'}},"ChangeMetadata":{"WereClientDataChanged":'+(case X2.WereClientDataChanged when null then 'null' when 1 then 'true' else 'false' end)
+',"WasCRSChanged":'+(case X2.WasCRSChanged when null then 'null' when 1 then 'true' else 'false' end)
+'}}'
from CustomerOnSA A
inner join (select A.CustomerOnSAId, B.* from dbo.CustomerOnSA A 
	CROSS APPLY OPENJSON (A.AdditionalData) 
	with (
		IsAddressWhispererUsed bit '$.IsAddressWhispererUsed',
		HasRelationshipWithKB bit '$.HasRelationshipWithKB',
		HasRelationshipWithKBEmployee bit '$.HasRelationshipWithKBEmployee',
		HasRelationshipWithCorporate bit '$.HasRelationshipWithCorporate',
		IsPoliticallyExposed bit '$.IsPoliticallyExposed',
		IsUSPerson bit '$.IsUSPerson',
		RestrictionTypeId int '$.LegalCapacity.RestrictionTypeId',
		RestrictionUntil_d int '$.LegalCapacity.RestrictionUntil.Day',
		RestrictionUntil_m int '$.LegalCapacity.RestrictionUntil.Month',
		RestrictionUntil_y int '$.LegalCapacity.RestrictionUntil.Year'
	) as B) as X1 on A.CustomerOnSAId=X1.CustomerOnSAId
inner join (select A.CustomerOnSAId, B.* from dbo.CustomerOnSA A 
	CROSS APPLY OPENJSON (A.ChangeMetadata) 
	with (
		WereClientDataChanged bit '$.WereClientDataChanged',
		WasCRSChanged bit '$.WasCRSChanged'
	) as B) as X2 on X1.CustomerOnSAId=X2.CustomerOnSAId
GO

SET IDENTITY_INSERT [DDS].[CustomerOnSAData] OFF;
GO

DROP TABLE IF EXISTS CustomerOnSA_backup;
GO
SELECT * INTO CustomerOnSA_backup FROM CustomerOnSA;
GO

ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = OFF  )
GO
ALTER TABLE [dbo].[CustomerOnSA] DROP COLUMN AdditionalDataBin;
GO
ALTER TABLE [dbo].[CustomerOnSA] DROP COLUMN AdditionalData;
GO
ALTER TABLE [dbo].[CustomerOnSA] DROP COLUMN ChangeMetadata;
GO
ALTER TABLE [dbo].[CustomerOnSA] DROP COLUMN ChangeMetadataBin;
GO
ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAHistory])  );
GO
