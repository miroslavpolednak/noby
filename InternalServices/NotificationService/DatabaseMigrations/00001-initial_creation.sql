BEGIN TRANSACTION;
GO

CREATE TABLE [EmailResult] (
    [Id] uniqueidentifier NOT NULL,
    [State] int NOT NULL,
    [Channel] int NOT NULL,
    [CustomId] nvarchar(450) NULL,
    [Identity] nvarchar(450) NULL,
    [IdentityScheme] nvarchar(450) NULL,
    [DocumentId] nvarchar(450) NULL,
    [RequestTimestamp] datetime2 NULL,
    [ResultTimestamp] datetime2 NULL,
    [Errors] nvarchar(max) NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_EmailResult] PRIMARY KEY ([Id])
    );
GO

CREATE TABLE [SmsResult] (
    [Id] uniqueidentifier NOT NULL,
    [State] int NOT NULL,
    [Channel] int NOT NULL,
    [CustomId] nvarchar(450) NULL,
    [Identity] nvarchar(450) NULL,
    [IdentityScheme] nvarchar(450) NULL,
    [DocumentId] nvarchar(450) NULL,
    [RequestTimestamp] datetime2 NULL,
    [ResultTimestamp] datetime2 NULL,
    [Errors] nvarchar(max) NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [CountryCode] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_SmsResult] PRIMARY KEY ([Id])
    );
GO

CREATE INDEX [IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId] ON [EmailResult] ([CustomId], [Identity], [IdentityScheme], [DocumentId]);
GO

CREATE INDEX [IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId] ON [SmsResult] ([CustomId], [Identity], [IdentityScheme], [DocumentId]);
GO

COMMIT;
GO