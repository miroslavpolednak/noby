UPDATE DocumentDataField 
SET StringFormat = 'Tato nabídka má pouze informační charakter a úroková sazba zde uvedená není námi garantována.


Seznam podkladů k Žádosti o úvěr je možné nalézt zde: www.kb.cz.' 
WHERE DocumentId = 1 AND AcroFieldName = 'Text'

DELETE FROM DocumentDynamicStringFormat WHERE DynamicStringFormatId = 13

GO

CREATE OR ALTER VIEW [dbo].[vw_DocumentDynamicStringFormats] AS

SELECT sf.DocumentId, sf.DocumentVersion, sf.AcroFieldName, sf.StringFormat, sf.[Priority], sf.DynamicStringFormatId, c.EqualToValue, df.FieldPath 
FROM DocumentDynamicStringFormat sf
LEFT JOIN DocumentDynamicStringFormatCondition c ON c.DynamicStringFormatId = sf.DynamicStringFormatId
LEFT JOIN DataField df ON df.DataFieldId = c.DataFieldId

GO