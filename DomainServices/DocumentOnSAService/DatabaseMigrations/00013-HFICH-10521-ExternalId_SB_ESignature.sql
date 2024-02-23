 ALTER TABLE [DocumentOnSa] ADD [ExternalIdSb] nvarchar(50) NULL;
 GO
 EXEC sp_rename N'[DocumentOnSa].[ExternalId]', N'ExternalIdESignatures', N'COLUMN';
 GO
 UPDATE dbo.DocumentOnSa
 SET ExternalIdSb = ExternalIdESignatures
 WHERE [Source]= 2;
 GO
 UPDATE dbo.DocumentOnSa
 SET ExternalIdESignatures = NULL
 WHERE [Source]= 2;