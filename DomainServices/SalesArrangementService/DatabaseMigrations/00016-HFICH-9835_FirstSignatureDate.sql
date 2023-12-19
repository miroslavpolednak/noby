ALTER TABLE dbo.SalesArrangement ADD FirstSignatureDate datetime2 NULL
GO
WITH SourceTable AS (

SELECT * FROM 
(
     SELECT 
     *
     FROM 
	 (
     SELECT 
     JSON_QUERY(sap.Parameters,'$.FirstSignatureDate') as FirstSignatureDateJson,
     CONVERT(DATETIME, CONCAT_WS('-',JSON_VALUE(sap.Parameters,'$.FirstSignatureDate.Year'),JSON_VALUE(sap.Parameters,'$.FirstSignatureDate.Month'),JSON_VALUE(sap.Parameters,'$.FirstSignatureDate.Day')), 120) as FirstSignatureDate,
     sa.SalesArrangementId
     FROM SalesArrangement sa
     INNER JOIN SalesArrangementParameters sap on sap.SalesArrangementId = sa.SalesArrangementId
     WHERE
     sa.SalesArrangementTypeId IN (1) -- Mortgage
     ) jdata
     WHERE jdata.FirstSignatureDateJson IS NOT NULL
) final 
)

MERGE INTO dbo.SalesArrangement AS target
USING SourceTable AS source
ON target.SalesArrangementId = source.SalesArrangementId
WHEN MATCHED THEN
    UPDATE SET target.FirstSignatureDate = source.FirstSignatureDate;