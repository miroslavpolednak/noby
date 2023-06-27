ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = OFF  );
GO
ALTER TABLE [dbo].[CustomerOnSA] ADD ChangeMetadata nvarchar(max);
ALTER TABLE [dbo].[CustomerOnSA] ADD ChangeMetadataBin varbinary(max);
ALTER TABLE [dbo].[CustomerOnSAHistory] ADD ChangeMetadata nvarchar(max);
ALTER TABLE [dbo].[CustomerOnSAHistory] ADD ChangeMetadataBin varbinary(max);
GO
ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAHistory])  );
GO