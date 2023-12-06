ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = OFF  )
GO
alter table [dbo].[RealEstateValuation] add PossibleValuationTypeId varchar(20) null;
GO
alter table [dbo].[RealEstateValuationHistory] add PossibleValuationTypeId varchar(20) null;
GO
ALTER TABLE [dbo].RealEstateValuation SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationHistory])  );
GO
