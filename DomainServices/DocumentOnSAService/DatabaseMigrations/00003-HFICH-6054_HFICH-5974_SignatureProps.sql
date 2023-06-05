IF COL_LENGTH('dbo.DocumentOnSa', 'IsDocumentArchived') IS NOT NULL
BEGIN
 EXEC sp_rename N'[DocumentOnSa].[IsDocumentArchived]', N'IsArchived', N'COLUMN';
END
GO

ALTER TABLE [DocumentOnSa] ADD [ExternalId] nvarchar(50) NULL;
GO

ALTER TABLE [DocumentOnSa] ADD [Source] int NOT NULL DEFAULT 1;
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EArchivIdsLinked]') AND type in (N'U'))
DROP TABLE [dbo].[EArchivIdsLinked]
GO

CREATE TABLE [EArchivIdsLinked] (
    [Id] int NOT NULL IDENTITY,
    [DocumentOnSAId] int NOT NULL,
    [EArchivId] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_EArchivIdsLinked] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EArchivIdsLinked_DocumentOnSa_DocumentOnSAId] FOREIGN KEY ([DocumentOnSAId]) REFERENCES [DocumentOnSa] ([DocumentOnSAId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_EArchivIdsLinked_DocumentOnSAId] ON [EArchivIdsLinked] ([DocumentOnSAId]);
GO