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

CREATE TABLE [ProcessedFiles] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    CONSTRAINT [PK_ProcessedFiles] PRIMARY KEY ([Id])
    );
GO

CREATE TABLE [ApplicationLogs] (
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
    CONSTRAINT [PK_ApplicationLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ApplicationLogs_ProcessedFiles_ProcessedFileId] FOREIGN KEY ([ProcessedFileId]) REFERENCES [ProcessedFiles] ([Id]) ON DELETE CASCADE
    );
GO

CREATE INDEX [IX_ApplicationLogs_ProcessedFileId] ON [ApplicationLogs] ([ProcessedFileId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230930181904_Initial', N'7.0.11');
GO

COMMIT;
GO
