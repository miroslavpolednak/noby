ALTER TABLE FormInstanceInterface ADD FORM_IDENTIFIER VARCHAR(25) NULL

GO

UPDATE FormInstanceInterface 
SET FormInstanceInterface.FORM_IDENTIFIER = t.RowNumber
FROM (SELECT ROW_NUMBER() OVER(Order by CREATED_AT) AS RowNumber, DOCUMENT_ID FROM FormInstanceInterface) as t
WHERE FormInstanceInterface.DOCUMENT_ID = t.DOCUMENT_ID

GO

ALTER TABLE FormInstanceInterface ALTER COLUMN FORM_IDENTIFIER VARCHAR(25) NOT NULL
ALTER TABLE FormInstanceInterface ADD CONSTRAINT AK_FormInstanceInterface_FormIdentifier UNIQUE (FORM_IDENTIFIER)