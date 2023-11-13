ALTER TABLE [dbo].[RealEstateValuation] ADD IsOnlineDisqualified bit default(0);
GO
UPDATE [dbo].[RealEstateValuation] SET IsOnlineDisqualified=0;
GO
