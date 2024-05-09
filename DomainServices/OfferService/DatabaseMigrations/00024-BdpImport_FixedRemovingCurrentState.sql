USE [OfferService]

GO

/****** Script for SelectTopNRows command from SSMS  ******/
CREATE OR ALTER   PROC [dbo].[DeleteRefixationOffer] 
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
AND o.OfferType = 2

-- Delete all non communicated (FlagState = 2) refixation offer
SELECT 
o.OfferId,
o.Flags
INTO #ForDelete
FROM dbo.Offer o
INNER JOIN #PairedCaseId p ON p.CaseId = o.CaseId
WHERE (o.Flags & @FlagState) != @FlagState AND o.OfferType = 2 --OfferType 2 > Refixation

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