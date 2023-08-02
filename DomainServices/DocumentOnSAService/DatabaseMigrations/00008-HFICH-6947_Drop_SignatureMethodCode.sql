DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DocumentOnSa]') AND [c].[name] = N'SignatureMethodCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [DocumentOnSa] DROP CONSTRAINT [' + @var0 + '];');
    
IF COL_LENGTH('dbo.DocumentOnSa', 'SignatureMethodCode') IS NOT NULL
BEGIN
    ALTER TABLE [DocumentOnSa] DROP COLUMN [SignatureMethodCode];
END

GO