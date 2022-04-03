ALTER TABLE [dbo].[SalesArrangement] SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].[SalesArrangementHistory];
TRUNCATE TABLE [dbo].[SalesArrangement];
ALTER TABLE [dbo].[SalesArrangement] SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[SalesArrangementHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].SalesArrangementParameters SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].SalesArrangementParametersHistory;
TRUNCATE TABLE [dbo].SalesArrangementParameters;
ALTER TABLE [dbo].SalesArrangementParameters SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[SalesArrangementParametersHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].CustomerOnSA SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].CustomerOnSAHistory;
TRUNCATE TABLE [dbo].CustomerOnSA;
ALTER TABLE [dbo].CustomerOnSA SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[CustomerOnSAHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].CustomerOnSAIdentity SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].CustomerOnSAIdentityHistory;
TRUNCATE TABLE [dbo].CustomerOnSAIdentity;
ALTER TABLE [dbo].CustomerOnSAIdentity SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[CustomerOnSAIdentityHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].CustomerOnSAIncome SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].CustomerOnSAIncomeHistory;
TRUNCATE TABLE [dbo].CustomerOnSAIncome;
ALTER TABLE [dbo].CustomerOnSAIncome SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[CustomerOnSAIncomeHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].CustomerOnSAObligations SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].CustomerOnSAObligationsHistory;
TRUNCATE TABLE [dbo].CustomerOnSAObligations;
ALTER TABLE [dbo].CustomerOnSAObligations SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[CustomerOnSAObligationsHistory], DATA_CONSISTENCY_CHECK = ON))  
GO

ALTER TABLE [dbo].Household SET (SYSTEM_VERSIONING = OFF)
TRUNCATE TABLE [dbo].HouseholdHistory;
TRUNCATE TABLE [dbo].Household;
ALTER TABLE [dbo].Household SET (SYSTEM_VERSIONING = On (HISTORY_TABLE =  [dbo].[HouseholdHistory], DATA_CONSISTENCY_CHECK = ON))  
GO
