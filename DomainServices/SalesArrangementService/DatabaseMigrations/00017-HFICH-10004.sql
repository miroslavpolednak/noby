ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = OFF  )
GO

alter table [dbo].[SalesArrangement] add PcpId varchar(40) NULL;
alter table [dbo].[SalesArrangementHistory] add PcpId varchar(40) NULL;
GO

ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SalesArrangementHistory] ))
GO