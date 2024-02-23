EXEC sp_rename 'DDS.OfferData', 'MortgageOfferData';
GO
EXEC sp_rename 'DDS.OfferDataHistory', 'MortgageOfferDataHistory';
GO
EXEC sp_rename 'DDS.AdditionalSimulationResultsData', 'MortgageAdditionalSimulationResultsData';
GO
EXEC sp_rename 'DDS.AdditionalSimulationResultsDataHistory', 'MortgageAdditionalSimulationResultsDataHistory';
GO
EXEC sp_rename 'DDS.CreditWorthinessSimpleData', 'MortgageCreditWorthinessSimpleData';
GO
EXEC sp_rename 'DDS.CreditWorthinessSimpleDataHistory', 'MortgageCreditWorthinessSimpleDataHistory';
GO

ALTER TABLE dbo.Offer ADD ValidTo datetime NULL;
GO
ALTER TABLE dbo.Offer ADD CaseId bigint NULL;
GO
ALTER TABLE dbo.Offer ADD SalesArrangementId int NULL;
GO
