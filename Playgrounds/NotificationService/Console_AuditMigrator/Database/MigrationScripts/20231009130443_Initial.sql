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

CREATE TABLE [ProcessedFile] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    CONSTRAINT [PK_ProcessedFile] PRIMARY KEY ([Id])
    );
GO

CREATE TABLE [ApplicationLog] (
    [Id] int NOT NULL IDENTITY,
    [LogType] int NOT NULL,
    [ProcessedFileId] int NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    [ThreadId] nvarchar(max) NULL,
    [Level] nvarchar(max) NULL,
    [TraceId] nvarchar(max) NULL,
    [SpanId] nvarchar(max) NULL,
    [ParentId] nvarchar(max) NULL,
    [CisAppKey] nvarchar(max) NULL,
    [Version] nvarchar(max) NULL,
    [Assembly] nvarchar(max) NULL,
    [SourceContext] nvarchar(max) NULL,
    [MachineName] nvarchar(max) NULL,
    [ClientIp] nvarchar(max) NULL,
    [CisUserId] nvarchar(max) NULL,
    [CisUserIdent] nvarchar(max) NULL,
    [RequestId] nvarchar(max) NULL,
    [RequestPath] nvarchar(max) NULL,
    [ConnectionId] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [Exception] nvarchar(max) NULL,
    CONSTRAINT [PK_ApplicationLog] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApplicationLog_ProcessedFile_ProcessedFileId] FOREIGN KEY ([ProcessedFileId]) REFERENCES [ProcessedFile] ([Id]) ON DELETE CASCADE
    );
GO

CREATE TABLE [MigrationData] (
    [Id] int NOT NULL IDENTITY,
    [ApplicationLogId] int NOT NULL,
    [LogType] int NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    [NotificationId] uniqueidentifier NULL,
    [RequestId] nvarchar(max) NULL,
    [Payload] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_MigrationData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MigrationData_ApplicationLog_ApplicationLogId] FOREIGN KEY ([ApplicationLogId]) REFERENCES [ApplicationLog] ([Id]) ON DELETE CASCADE
    );
GO

CREATE INDEX [IX_ApplicationLog_ProcessedFileId] ON [ApplicationLog] ([ProcessedFileId]);
GO

CREATE INDEX [IX_MigrationData_ApplicationLogId] ON [MigrationData] ([ApplicationLogId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231009130443_Initial', N'7.0.11');
GO

COMMIT;
GO
