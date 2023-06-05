BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SmsResult]') AND [c].[name] = N'Text');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [SmsResult] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [SmsResult] DROP COLUMN [Text];
GO

COMMIT;
GO
