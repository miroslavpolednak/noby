ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = OFF  )
GO

alter table [dbo].[SalesArrangement] add LoanApplicationDataVersion varchar(50) NULL;
alter table [dbo].[SalesArrangementHistory] add LoanApplicationDataVersion varchar(50) NULL;
GO

ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SalesArrangementHistory] ))
GO