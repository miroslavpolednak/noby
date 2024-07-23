alter table [dbo].[ConfirmedPriceException] add DeclinedDate date null;
GO
ALTER TABLE [dbo].[ConfirmedPriceException] ALTER COLUMN ConfirmedDate date NULL
GO