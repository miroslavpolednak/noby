IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221222163946_Initial', N'7.0.1');
GO

COMMIT;
GO