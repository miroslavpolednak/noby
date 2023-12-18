DROP TABLE IF EXISTS Offer_backup;
GO
SELECT * INTO Offer_backup FROM Offer;
GO

ALTER TABLE [dbo].[Offer] DROP COLUMN BasicParameters;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationInputs;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationResults;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN AdditionalSimulationResults;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN BasicParametersBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationInputsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN SimulationResultsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN AdditionalSimulationResultsBin;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN CreditWorthinessSimpleInputs;
GO
ALTER TABLE [dbo].[Offer] DROP COLUMN CreditWorthinessSimpleInputsBin;
GO
