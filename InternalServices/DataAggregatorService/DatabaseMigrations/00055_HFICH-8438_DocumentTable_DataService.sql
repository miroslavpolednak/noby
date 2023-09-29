CREATE OR ALTER VIEW [dbo].[vw_DocumentTables] AS

SELECT t.DocumentTableId, t.DocumentId, t.DocumentVersion, df.DataServiceId, df.FieldPath as TableSourcePath, t.AcroFieldPlaceholderName, t.ConcludingParagraph
FROM DocumentTable t
INNER JOIN DataField df ON df.DataFieldId = t.DataFieldId