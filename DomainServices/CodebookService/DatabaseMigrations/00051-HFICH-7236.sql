ALTER TABLE [dbo].[SalesArrangementType] ADD IsFormSentToCmp BIT NOT NULL DEFAULT 0

GO

UPDATE [dbo].[SalesArrangementType] SET IsFormSentToCmp = 1 WHERE Id IN (1, 6, 10, 11, 12)

UPDATE [dbo].[SqlQuery] 
SET SqlQueryText = 'SELECT Id, Name, SalesArrangementCategory, Description, IsFormSentToCmp FROM [dbo].[SalesArrangementType] ORDER BY Id'
WHERE SqlQueryId = 'SalesArrangementTypes'