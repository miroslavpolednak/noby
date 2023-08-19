ALTER TABLE DocumentTemplateVersion ADD TemplateProcessingType VARCHAR(5) NOT NULL DEFAULT 'A'

GO

UPDATE SqlQuery 
SET SqlQueryText = 'SELECT Id, DocumentTypeId, DocumentVersion, FormTypeId, TemplateProcessingType, CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[DocumentTemplateVersion] ORDER BY Id ASC' 
WHERE SqlQueryId = 'DocumentTemplateVersions'