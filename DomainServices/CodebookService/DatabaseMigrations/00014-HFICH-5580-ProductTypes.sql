-- ProductTypeExtension
ALTER TABLE [dbo].[ProductTypeExtension] ADD [MandantId] int NULL;
GO
UPDATE [dbo].[ProductTypeExtension] SET [MandantId] = 2;
GO
ALTER TABLE [dbo].[ProductTypeExtension] ALTER COLUMN [MandantId] INT NOT NULL;
GO