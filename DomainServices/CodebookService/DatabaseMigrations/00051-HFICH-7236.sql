ALTER TABLE [dbo].[SalesArrangementType] ADD IsFormSentToCmp BIT NOT NULL DEFAULT 0

GO

UPDATE [dbo].[SalesArrangementType] SET IsFormSentToCmp = 1 WHERE Id IN (1, 6, 10, 11, 12)