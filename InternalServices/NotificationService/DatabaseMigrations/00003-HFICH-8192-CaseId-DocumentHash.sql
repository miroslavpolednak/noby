BEGIN TRANSACTION;
GO

DROP INDEX [IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId] ON [SmsResult];
GO

DROP INDEX [IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId] ON [EmailResult];
GO

ALTER TABLE [SmsResult] ADD [CaseId] bigint NULL;
GO

ALTER TABLE [SmsResult] ADD [DocumentHash] nvarchar(max) NULL;
GO

ALTER TABLE [SmsResult] ADD [HashAlgorithm] nvarchar(max) NULL;
GO

ALTER TABLE [EmailResult] ADD [CaseId] bigint NULL;
GO

ALTER TABLE [EmailResult] ADD [DocumentHash] nvarchar(max) NULL;
GO

ALTER TABLE [EmailResult] ADD [HashAlgorithm] nvarchar(max) NULL;
GO

CREATE INDEX [IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId] ON [SmsResult] ([CustomId], [Identity], [IdentityScheme], [DocumentId], [CaseId]);
GO

CREATE INDEX [IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId] ON [EmailResult] ([CustomId], [Identity], [IdentityScheme], [DocumentId], [CaseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230920095629_HFICH-8192-CaseId-DocumentHash', N'7.0.9');
GO

COMMIT;
GO
