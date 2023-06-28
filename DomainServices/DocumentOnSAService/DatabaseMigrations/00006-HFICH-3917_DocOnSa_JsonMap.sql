IF COL_LENGTH('dbo.DocumentOnSa', 'CaseId') IS NULL
BEGIN
ALTER TABLE [DocumentOnSa] ADD [CaseId] bigint NULL;
END
GO

IF COL_LENGTH('dbo.DocumentOnSa', 'CustomerOnSAId1') IS NULL
BEGIN
ALTER TABLE [DocumentOnSa] ADD [CustomerOnSAId1] int NULL;
END
GO

IF COL_LENGTH('dbo.DocumentOnSa', 'CustomerOnSAId2') IS NULL
BEGIN
ALTER TABLE [DocumentOnSa] ADD [CustomerOnSAId2] int NULL;
END
GO

IF COL_LENGTH('dbo.DocumentOnSa', 'IsPreviewSentToCustomer') IS NULL
BEGIN
ALTER TABLE [DocumentOnSa] ADD [IsPreviewSentToCustomer] bit NOT NULL DEFAULT CAST(0 AS bit);
END
GO

IF COL_LENGTH('dbo.DocumentOnSa', 'TaskId') IS NULL
BEGIN
ALTER TABLE [DocumentOnSa] ADD [TaskId] int NULL;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SigningIdentity]') AND type in (N'U'))
DROP TABLE [dbo].[SigningIdentity]
GO

CREATE TABLE [SigningIdentity] (
    [Id] int NOT NULL IDENTITY,
    [DocumentOnSAId] int NOT NULL,
    [SigningIdentityJson] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_SigningIdentity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SigningIdentity_DocumentOnSa_DocumentOnSAId] FOREIGN KEY ([DocumentOnSAId]) REFERENCES [DocumentOnSa] ([DocumentOnSAId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_SigningIdentity_DocumentOnSAId] ON [SigningIdentity] ([DocumentOnSAId]);
GO
