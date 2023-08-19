ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = OFF  )
GO

ALTER TABLE [dbo].[RealEstateValuation] ADD Documents nvarchar(max) NULL;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] ADD Documents nvarchar(max) NULL;
GO

ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationHistory])  )
GO
