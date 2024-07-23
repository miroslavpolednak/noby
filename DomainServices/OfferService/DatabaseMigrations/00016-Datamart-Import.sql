/****** Object:  Table [dbo].[OfferRefixationBatchRelation]    Script Date: 05.04.2024 15:29:40 ******/
DROP TABLE IF EXISTS [dbo].[OfferRefixationBatchRelation]
GO
/****** Object:  Table [dbo].[NonPairedItems]    Script Date: 05.04.2024 15:29:40 ******/
DROP TABLE IF EXISTS [dbo].[NonPairedItems]
GO
/****** Object:  StoredProcedure [dbo].[ImportDataFromDatamart]    Script Date: 05.04.2024 15:29:40 ******/
DROP PROCEDURE IF EXISTS [dbo].[ImportDataFromDatamart]
GO
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOffer]    Script Date: 05.04.2024 15:29:40 ******/
DROP PROCEDURE IF EXISTS [dbo].[DeleteRefixationOffer]
GO
/****** Object:  StoredProcedure [dbo].[DeleteDatamartStageTables]    Script Date: 05.04.2024 15:29:40 ******/
DROP PROCEDURE IF EXISTS [dbo].[DeleteDatamartStageTables]
GO
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOfferOlderThan]    Script Date: 09.04.2024 10:36:23 ******/
DROP PROCEDURE IF EXISTS [dbo].[DeleteRefixationOfferOlderThan]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NonPairedItems](
	[BatchId] [bigint] NOT NULL,
	[AccountNbr] [nchar](16) NOT NULL,
	[OfferValidTo] [date] NULL,
	[Data] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfferRefixationBatchRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResourceProcessId] [uniqueidentifier] NULL,
	[BatchId] [bigint] NULL,
 CONSTRAINT [PK_OfferRefixationBatchRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[DeleteDatamartStageTables]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[DeleteDatamartStageTables]
@BatchId BIGINT
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
SET NOCOUNT ON;

DELETE [bdp].[D_CUST_RETENTION_ACCOUNT] WHERE Batch_Id = @BatchId
DELETE [bdp].[D_CUST_RETENTION_OFFER] WHERE Batch_Id = @BatchId

COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction
    ROLLBACK TRANSACTION;
    -- Handle the error
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOffer]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE OR ALTER PROC [dbo].[DeleteRefixationOffer] 
  @FlagState INT,
  @BatchId BIGINT
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
SET NOCOUNT ON;

SELECT 
k.Id AS CaseId,
oin.Account_Nbr AS Account_Nbr
INTO #PairedCaseId
FROM bdp.D_CUST_RETENTION_ACCOUNT oin 
INNER JOIN bdp.KONSTDB_UVER_S k ON k.Neaktivni = 0 AND CAST(CAST(CONCAT(k.PredcisliUctu, RIGHT(REPLICATE('0', 10) + CONVERT(NVARCHAR(10), k.CisloUctu), 10)) AS BIGINT) AS VARCHAR) = CAST(CAST(oin.Account_Nbr AS BIGINT) AS VARCHAR) 
WHERE oin.Batch_Id = @BatchId

-- If some communicated offer have state Current, we have to romeve this state from flag 
UPDATE o
SET o.Flags = o.Flags - 1 
FROM OFFER o
INNER JOIN #PairedCaseId p ON p.CaseId = o.CaseId
WHERE (o.Flags & @FlagState) = @FlagState 
AND (o.Flags & 1) = 1 -- Current
AND o.OfferType = 2 AND o.Origin = 1

-- Delete all non communicated (FlagState = 2) refixation offer
SELECT 
o.OfferId,
o.Flags
INTO #ForDelete
FROM dbo.Offer o
INNER JOIN #PairedCaseId p ON p.CaseId = o.CaseId
WHERE (o.Flags & @FlagState) != @FlagState AND o.OfferType = 2 AND o.Origin = 1 --OfferType 2 > Refixation -- Origin 1 > BigDataPlatform

DELETE o
FROM dbo.Offer o
INNER JOIN #ForDelete d ON d.OfferId = o.OfferId

DELETE r
FROM DDS.MortgageRefixationData r
INNER JOIN #ForDelete d ON d.OfferId = r.DocumentDataEntityId

DROP TABLE #ForDelete
DROP TABLE #PairedCaseId

COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction
    ROLLBACK TRANSACTION;
    -- Handle the error
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[ImportDataFromDatamart]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[ImportDataFromDatamart] 
  @BatchSize INT,
  @BatchId BIGINT
AS
BEGIN
SET NOCOUNT ON;
;WITH CTE AS (
SELECT TOP (@BatchSize) 
COALESCE(k.Id, 0) AS CaseId,
oin.Account_Nbr AS Account_Nbr,
oin.Batch_Id AS BatchId,
ain.Refixation_Date AS BasicParameters_FixedRateValidTo,
oin.Offer_Type AS RefixationOfferTypeId,
oin.Repayment_Amt AS SimulationResults_LoanPaymentAmount,
oin.Interest_Rate AS SimulationInputs_InterestRate,
oin.Fixation_Period AS SimulationInputs_FixedRatePeriod,
oin.Interest_Rate_Valid_To AS Offer_ValidTo
FROM bdp.D_CUST_RETENTION_OFFER oin 
INNER JOIN bdp.D_CUST_RETENTION_ACCOUNT ain ON oin.Was_Processed_By_Noby = 0 AND oin.Account_Nbr = ain.Account_Nbr AND ain.Batch_Id = oin.Batch_Id
LEFT JOIN dbo.KONSTDB_UVER_S k ON k.Neaktivni = 0 AND CAST(CAST(CONCAT(k.PredcisliUctu, RIGHT(REPLICATE('0', 10) + CONVERT(NVARCHAR(10), k.CisloUctu), 10)) AS BIGINT) AS VARCHAR) = CAST(CAST(oin.Account_Nbr AS BIGINT) AS VARCHAR) 
WHERE oin.Batch_Id = @BatchId
)

SELECT 
c.BatchId,
c.CaseId,
c.Account_Nbr,
c.Offer_ValidTo,
2 as OfferType, --  MortgageRefixation
c.SimulationInputs_FixedRatePeriod,
NEWID() AS ResourceProcessId,
Flags = CASE
 WHEN c.RefixationOfferTypeId = 1 THEN 1
 WHEN c.RefixationOfferTypeId = 3 THEN 4
 END,
1 AS Origin, -- BigDataPlatform
(
  SELECT 
  cin.SimulationResults_LoanPaymentAmount AS 'SimulationResults.LoanPaymentAmount',
  cin.SimulationInputs_InterestRate AS 'SimulationInputs.InterestRate',
  cin.SimulationInputs_FixedRatePeriod AS 'SimulationInputs.FixedRatePeriod',
  cin.BasicParameters_FixedRateValidTo AS 'BasicParametersData.FixedRateValidTo'
  FROM CTE cin WHERE cin.Account_Nbr =c.Account_Nbr AND cin.SimulationInputs_FixedRatePeriod = c.SimulationInputs_FixedRatePeriod
  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
) as JsonData
INTO #TempDataForImport
FROM CTE c
ORDER BY c.CaseId

--SELECT * FROM #TempDataForImport -- ToDo comment in real use is for debugging purpose only

BEGIN TRANSACTION;
BEGIN TRY
    DECLARE @ResourceProcessId uniqueidentifier
	SET @ResourceProcessId = NEWID();

	DECLARE @CurrentDate DATETIME;
	SET @CurrentDate = GETDATE();
	
	-- table with created offerId for inserting to related table  
	DECLARE @OutputTable TABLE (OfferId INT, ResourceProcessId uniqueidentifier)

	--Offer insert
	INSERT INTO dbo.Offer (ResourceProcessId, CreatedTime, IsCreditWorthinessSimpleRequested, ValidTo, CaseId, OfferType, Flags, Origin)
    OUTPUT INSERTED.OfferId, INSERTED.ResourceProcessId INTO @OutputTable
	SELECT 
	ti.ResourceProcessId AS ResourceProcessId,
	@CurrentDate AS CreatedTime,
	0 AS IsCreditWorthinessSimpleRequested,
	ti.Offer_ValidTo AS ValidTo,
	ti.CaseId AS CaseId,
	ti.OfferType AS OfferType,
	ti.Flags AS Flags,
	ti.Origin AS Origin
	FROM #TempDataForImport ti
	WHERE ti.CaseId > 0
	
	--select * from @OutputTable  -- ToDo comment in real use is for debugging purpose only

	-- This table join new offer id with jsonData and prepare data for import  
	SELECT 
	CONVERT(VARCHAR(50), o.OfferId) AS DocumentDataEntityId, 
	1 AS DocumentDataVersion,
	t.JsonData AS [Data],
	1 AS CreatedUserId,
	@CurrentDate AS CreatedTime
	INTO #TempJdonDataForImport
	FROM @OutputTable o
	INNER JOIN #TempDataForImport t ON o.ResourceProcessId = t.ResourceProcessId

	--select * from #TempJdonDataForImport  -- ToDo comment in real use is for debugging purpose only

	--MortgageRefixationData insert
	INSERT INTO DDS.MortgageRefixationData(DocumentDataEntityId, DocumentDataVersion, [Data], CreatedUserId, CreatedTime)
	SELECT
	tj.DocumentDataEntityId,
	tj.DocumentDataVersion,
	tj.[Data],
	tj.CreatedUserId,
	tj.CreatedTime
	FROM #TempJdonDataForImport tj

	-- We used ResourceProcessId temporarily like unique identifier for row, but this isn't purpose of this column, should be only one identifier per batch, so we have to set only one per batch
	UPDATE o
	SET o.ResourceProcessId = @ResourceProcessId
	FROM dbo.Offer o
	INNER JOIN @OutputTable ot ON o.ResourceProcessId = ot.ResourceProcessId  
	
	-- Set was processed by noby to true for transaction batch
	UPDATE a
	SET a.Was_Processed_By_Noby = 1
	FROM bdp.D_CUST_RETENTION_ACCOUNT a
	INNER JOIN #TempDataForImport ot ON a.Account_Nbr = ot.Account_Nbr AND a.Batch_Id = ot.BatchId  
	
	-- Log non paired items (CaseId is null so we can't processed those data)
	INSERT INTO dbo.NonPairedItems (BatchId, AccountNbr, OfferValidTo, [Data])
    SELECT 
	ti.BatchId AS BatchId,
	ti.Account_Nbr AS AccountNbr,
	ti.Offer_ValidTo AS OfferValidTo,
	ti.JsonData AS [Data]
	FROM #TempDataForImport ti
	WHERE ti.CaseId = 0
	
	-- Add row to OfferRetentionBatchRelation
	INSERT INTO dbo.OfferRefixationBatchRelation (ResourceProcessId, BatchId)
	SELECT TOP(1)
	@ResourceProcessId,
	ti.BatchId
	FROM #TempDataForImport ti
	
	COMMIT TRANSACTION;
	-- If all data per BatchId from datamart was processed , we set Was_Processed_By_Noby to true for whole batch (so datamart knows, that can import new batch and we knows, that whole batch was processed) 
	IF (SELECT COUNT(*) FROM bdp.D_CUST_RETENTION_OFFER a WHERE a.Batch_Id = @BatchId AND a.Was_Processed_By_Noby = 0) = 0
		BEGIN
		UPDATE b
		SET b.Was_Processed_By_Noby = 1
		FROM bdp.D_CUST_RETENTION_BATCH b
		WHERE b.Batch_Id = @BatchId 
	END
END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction
    ROLLBACK TRANSACTION;
    -- Handle the error
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;

DROP TABLE #TempDataForImport
DROP TABLE #TempJdonDataForImport

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOfferOlderThan]    Script Date: 09.04.2024 10:36:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[DeleteRefixationOfferOlderThan] 
  @Date DATE
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
SET NOCOUNT ON;

SELECT 
o.OfferId,
o.Flags
INTO #ForDelete
FROM dbo.Offer o
WHERE o.ValidTo < @Date AND o.OfferType = 2 AND o.Origin = 1 --OfferType 2 > Refixation -- Origin 1 > BigDataPlatform

DELETE o
FROM dbo.Offer o
INNER JOIN #ForDelete d ON d.OfferId = o.OfferId

DELETE r
FROM DDS.MortgageRefixationData r
INNER JOIN #ForDelete d ON d.OfferId = r.DocumentDataEntityId

DROP TABLE #ForDelete

COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction
    ROLLBACK TRANSACTION;
    -- Handle the error
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;
END
GO