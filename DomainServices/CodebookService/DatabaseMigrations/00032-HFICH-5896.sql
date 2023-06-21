ALTER TABLE dbo.DocumentTypes DROP COLUMN IsFormIdRequested;
GO
UPDATE [dbo].[SqlQuery] SET [SqlQueryText]='SELECT [Id], Id ''EnumValue'', [ShortName],[Name],[FileName],[SalesArrangementTypeId],[EACodeMainId],[IsFormIdRequested], CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[DocumentTypes] ORDER BY [Id]' WHERE SqlQueryId='DocumentTypes';
GO
