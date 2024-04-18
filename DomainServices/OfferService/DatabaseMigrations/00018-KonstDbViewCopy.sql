DROP TABLE IF EXISTS [dbo].[CaseIdAccountNumberKonstDb]
GO
DROP TABLE IF EXISTS  [dbo].[NonPairedItems]
GO
/****** Object:  StoredProcedure [dbo].[ImportKonstDbView]    Script Date: 17.04.2024 15:13:49 ******/
DROP PROCEDURE IF EXISTS [dbo].[ImportKonstDbView]
GO
/****** Object:  Table [dbo].[CaseIdAccountNumberKonstDb]    Script Date: 17.04.2024 15:10:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseIdAccountNumberKonstDb](
	[CaseId] [bigint] NULL,
	[AreaCodeAccountNumber] [nchar](16) NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_CaseIdAccountNumberKonstDb]    Script Date: 17.04.2024 15:11:59 ******/
CREATE NONCLUSTERED INDEX [IX_CaseIdAccountNumberKonstDb] ON [dbo].[CaseIdAccountNumberKonstDb]
(
	[AreaCodeAccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[ImportKonstDbView]    Script Date: 17.04.2024 15:13:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[ImportKonstDbView]
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
SET NOCOUNT ON;

TRUNCATE TABLE dbo.CaseIdAccountNumberKonstDb

INSERT INTO dbo.CaseIdAccountNumberKonstDb(CaseId, AreaCodeAccountNumber)
SELECT 
k.UverId,
k.PredcisliCisloUctu
FROM [dbo].[KonsDb_UverCisloUctuView_S] k
WHERE k.PredcisliCisloUctu IS NOT NULL AND k.PredcisliCisloUctu <> ''

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
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOffer]    Script Date: 17.04.2024 16:21:19 ******/
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
k.CaseId AS CaseId,
oin.Account_Nbr AS Account_Nbr
INTO #PairedCaseId
FROM bdp.D_CUST_RETENTION_ACCOUNT oin 
INNER JOIN dbo.CaseIdAccountNumberKonstDb k ON k.AreaCodeAccountNumber = oin.Account_Nbr 
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
CREATE TABLE [dbo].[NonPairedItems](
	[BatchId] [bigint] NOT NULL,
	[AccountNbr] [nchar](16) NOT NULL,
	[OfferValidTo] [date] NULL,
	[Data] [nvarchar](max) NULL,
	[CustomerChurnRisk] [numeric](16, 4) NULL,
	[CustomerPriceSensitivity] [numeric](16, 4) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ImportDataFromDatamart]    Script Date: 18.04.2024 13:54:52 ******/
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
SELECT TOP (@BatchSize) 
COALESCE(k.CaseId, 0) AS CaseId,
oin.Account_Nbr AS Account_Nbr,
oin.Batch_Id AS BatchId,
ain.Refixation_Date AS BasicParameters_FixedRateValidTo,
oin.Offer_Type AS RefixationOfferTypeId,
oin.Installment_Amount AS SimulationResults_LoanPaymentAmount,
oin.Interest_Rate AS SimulationInputs_InterestRate,
oin.Fixation_Period AS SimulationInputs_FixedRatePeriod,
oin.Interest_Rate_Valid_To AS Offer_ValidTo,
oin.Default_Choice_Flag AS IsLegalNoticeDefault,
oin.Offer_Date AS BasicParametersData_LegalNoticeGeneratedDate,
ain.Leave_Probability AS Case_Customer_Churn_Risk,
ain.Calculated_Delta AS Case_Customer_Price_Sensitivity
INTO #PairedData
FROM bdp.D_CUST_RETENTION_OFFER oin 
INNER JOIN bdp.D_CUST_RETENTION_ACCOUNT ain ON oin.Was_Processed_By_Noby = 0 AND oin.Account_Nbr = ain.Account_Nbr AND ain.Batch_Id = oin.Batch_Id
LEFT JOIN dbo.CaseIdAccountNumberKonstDb k ON k.AreaCodeAccountNumber = oin.Account_Nbr 
WHERE oin.Batch_Id = @BatchId

SELECT 
c.BatchId,
c.CaseId,
c.Account_Nbr,
c.Offer_ValidTo,
2 as OfferType, --  MortgageRefixation
c.SimulationInputs_FixedRatePeriod,
c.IsLegalNoticeDefault,
c.Case_Customer_Churn_Risk,
c.Case_Customer_Price_Sensitivity,
NEWID() AS ResourceProcessId,
Flags = CASE
 WHEN c.RefixationOfferTypeId = 1 AND c.IsLegalNoticeDefault = 1 THEN 33 -- Current(1) + LegalNoticeDefault(32)  
 WHEN c.RefixationOfferTypeId = 3 AND c.IsLegalNoticeDefault = 1 THEN 36 -- LegalNotice(4) + LegalNoticeDefault(32)
 WHEN c.RefixationOfferTypeId = 1 THEN 1
 WHEN c.RefixationOfferTypeId = 3 THEN 4
 END,
1 AS Origin, -- BigDataPlatform
(
  SELECT 
  cin.SimulationResults_LoanPaymentAmount AS 'SimulationResults.LoanPaymentAmount',
  cin.SimulationInputs_InterestRate AS 'SimulationInputs.InterestRate',
  cin.SimulationInputs_FixedRatePeriod AS 'SimulationInputs.FixedRatePeriod',
  cin.BasicParameters_FixedRateValidTo AS 'BasicParametersData.FixedRateValidTo',
  cin.BasicParametersData_LegalNoticeGeneratedDate AS 'BasicParametersData.LegalNoticeGeneratedDate'
  FROM #PairedData cin WHERE cin.Account_Nbr =c.Account_Nbr AND cin.SimulationInputs_FixedRatePeriod = c.SimulationInputs_FixedRatePeriod
  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
) as JsonData
INTO #TempDataForImport
FROM #PairedData c
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
	UPDATE o
	SET o.Was_Processed_By_Noby = 1
	FROM bdp.D_CUST_RETENTION_OFFER o
	INNER JOIN #TempDataForImport ot ON o.Account_Nbr = ot.Account_Nbr AND o.Batch_Id = ot.BatchId AND o.Fixation_Period =ot.SimulationInputs_FixedRatePeriod 
	
	-- Update customerPriceSensitivity and customerChurnRisk On CaseService db (is on same db server)
	UPDATE c
	SET 
	c.CustomerChurnRisk = p.Case_Customer_Churn_Risk,
	c.CustomerPriceSensitivity = p.Case_Customer_Price_Sensitivity
	FROM [CaseService].[dbo].[Case] c
	INNER JOIN #PairedData p ON c.CaseId = p.CaseId

	-- Log non paired items (CaseId is null so we can't processed those data)
	INSERT INTO dbo.NonPairedItems (BatchId, AccountNbr, OfferValidTo, [Data], CustomerChurnRisk, CustomerPriceSensitivity)
    SELECT 
	ti.BatchId AS BatchId,
	ti.Account_Nbr AS AccountNbr,
	ti.Offer_ValidTo AS OfferValidTo,
	ti.JsonData AS [Data],
	ti.Case_Customer_Churn_Risk,
	ti.Case_Customer_Price_Sensitivity
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
	IF (SELECT COUNT(*) FROM bdp.D_CUST_RETENTION_OFFER o WHERE o.Batch_Id = @BatchId AND o.Was_Processed_By_Noby = 0) = 0
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
DROP TABLE #PairedData
END


