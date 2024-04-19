ALTER TABLE SalesArrangementType ADD FeeId INT NULL

GO

UPDATE SalesArrangementType SET FeeId = 2009 WHERE Id = 13
UPDATE SalesArrangementType SET FeeId = 2033 WHERE Id = 15

UPDATE SqlQuery 
SET SqlQueryText = 'SELECT Id, Name, SalesArrangementCategory, Description, IsFormSentToCmp, FeeId FROM [dbo].[SalesArrangementType] ORDER BY Id'
WHERE SqlQueryId = 'SalesArrangementTypes'