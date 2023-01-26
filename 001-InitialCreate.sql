BEGIN TRANSACTION;
GO

CREATE TABLE [DocumentOnSa] (
    [DocumentOnSAId] int NOT NULL IDENTITY,
    [DocumentTypeId] int NOT NULL,
    [DocumentTemplateVersionId] int NULL,
    [FormId] nvarchar(15) NOT NULL,
    [EArchivId] nvarchar(50) NULL,
    [DmsxId] nvarchar(50) NULL,
    [SalesArrangementId] int NOT NULL,
    [HouseholdId] int NOT NULL,
    [IsValid] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IsSigned] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsDocumentArchived] bit NOT NULL DEFAULT CAST(0 AS bit),
    [SignatureMethodCode] nvarchar(15) NOT NULL,
    [SignatureDateTime] datetime2 NULL,
    [SignatureConfirmedBy] int NULL,
    [CreatedUserName] nvarchar(50) NULL,
    [CreatedUserId] int NULL,
    [CreatedTime] datetime2 NOT NULL,
    [Data] nvarchar(MAX) NOT NULL,
    CONSTRAINT [PK_DocumentOnSa] PRIMARY KEY ([DocumentOnSAId])
);
GO

CREATE TABLE [GeneratedFormId] (
    [Id] bigint NOT NULL IDENTITY,
    [HouseholdId] int NULL,
    [Version] smallint NOT NULL,
    [IsFormIdFinal] bit NOT NULL,
    [TargetSystem] nvarchar(2) NOT NULL DEFAULT N'N',
    CONSTRAINT [PK_GeneratedFormId] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_DocumentOnSa_SalesArrangementId] ON [DocumentOnSa] ([SalesArrangementId]);
GO

CREATE INDEX [IX_GeneratedFormId_HouseholdId] ON [GeneratedFormId] ([HouseholdId]);
GO

COMMIT;
GO
