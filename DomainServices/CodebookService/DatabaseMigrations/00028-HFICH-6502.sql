ALTER TABLE [dbo].[SalesArrangementType] DROP COLUMN ProductTypeId;
GO
DELETE FROM [dbo].[SalesArrangementType] WHERE Id IN (2,3,4,5);
UPDATE [dbo].[SqlQuery] SET SqlQueryText='SELECT Id, Name, SalesArrangementCategory, Description FROM [dbo].[SalesArrangementType] ORDER BY Id' WHERE SqlQueryId='SalesArrangementTypes';
GO
