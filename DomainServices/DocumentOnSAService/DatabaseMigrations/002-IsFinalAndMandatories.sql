BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DocumentOnSa]') AND [c].[name] = N'SignatureMethodCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [DocumentOnSa] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [DocumentOnSa] ALTER COLUMN [SignatureMethodCode] nvarchar(15) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DocumentOnSa]') AND [c].[name] = N'HouseholdId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [DocumentOnSa] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [DocumentOnSa] ALTER COLUMN [HouseholdId] int NULL;
GO

ALTER TABLE [DocumentOnSa] ADD [IsFinal] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

COMMIT;
GO