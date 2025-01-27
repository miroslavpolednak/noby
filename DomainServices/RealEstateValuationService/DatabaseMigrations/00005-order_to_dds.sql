IF NOT EXISTS ( SELECT  schema_name FROM    information_schema.schemata WHERE   schema_name = 'DDS' )
BEGIN
EXEC sp_executesql N'CREATE SCHEMA DDS'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DDS].[RealEstateValuationOrderData]') AND type in (N'U'))
ALTER TABLE [DDS].[RealEstateValuationOrderData] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [DDS].[RealEstateValuationOrderData]
GO
DROP TABLE IF EXISTS [DDS].[RealEstateValuationOrderDataHistory]
GO
CREATE TABLE [DDS].[RealEstateValuationOrderData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL
 CONSTRAINT [PK_RealEstateValuationOrderData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
	SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[RealEstateValuationOrderDataHistory])
)
GO

DROP INDEX IF EXISTS [Idx_EntityId] ON [DDS].[RealEstateValuationOrderData]
GO
CREATE NONCLUSTERED INDEX [Idx_EntityId] ON [DDS].[RealEstateValuationOrderData]
(
	[DocumentDataEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

INSERT INTO [DDS].[RealEstateValuationOrderData]
([DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.RealEstateValuationOrderId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"RealEstateValuationOrderType":2,"Standard":'+A.[Data]+'}'
from [dbo].[RealEstateValuationOrder] A
where RealEstateValuationOrderType=0 
GO

INSERT INTO [DDS].[RealEstateValuationOrderData]
([DocumentDataEntityId],[DocumentDataVersion],[CreatedUserId],[CreatedTime],[ModifiedUserId],[Data])
select A.RealEstateValuationOrderId, 1, A.CreatedUserId, A.CreatedTime, A.ModifiedUserId, 
'{"RealEstateValuationOrderType":1,"OnlinePreorder":'+A.[Data]+'}'
from [dbo].[RealEstateValuationOrder] A
where RealEstateValuationOrderType=1
GO

ALTER TABLE [dbo].[RealEstateValuationOrder] SET ( SYSTEM_VERSIONING = OFF  )
GO
drop table RealEstateValuationOrderHistory;
GO
