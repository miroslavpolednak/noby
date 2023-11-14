ALTER TABLE [dbo].[ProfessionCategoryExtension] ADD IsValidNoby bit NULL;
GO
UPDATE [dbo].[ProfessionCategoryExtension] SET IsValidNoby=1;
INSERT INTO [dbo].[ProfessionCategoryExtension] (ProfessionCategoryId, IsValidNoby) VALUES (0, 0), (7, 0);
GO
