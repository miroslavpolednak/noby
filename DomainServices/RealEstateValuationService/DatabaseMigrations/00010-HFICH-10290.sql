drop table if exists RealEstateValuationOld
GO
select * into RealEstateValuationOld from RealEstateValuation where ValuationResultCurrentPrice is not null or ValuationResultFuturePrice is not null;
GO

ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = OFF  )
GO

ALTER TABLE [dbo].[RealEstateValuation] DROP COLUMN ValuationResultCurrentPrice;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN ValuationResultCurrentPrice;
GO
ALTER TABLE [dbo].[RealEstateValuation] DROP COLUMN ValuationResultFuturePrice;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] DROP COLUMN ValuationResultFuturePrice;
GO
ALTER TABLE [dbo].[RealEstateValuation] ADD Prices varchar(max) NULL;
GO
ALTER TABLE [dbo].[RealEstateValuationHistory] ADD Prices varchar(max) NULL;
GO

ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationHistory])  )
GO

update [dbo].[RealEstateValuation] set Prices=
'['
+(case when B.ValuationResultCurrentPrice is null then '' else '{"Price":'+cast(B.ValuationResultCurrentPrice as varchar(20))+',"PriceSourceType":"STANDARD_PRICE_EXIST"}' end)
+(case when B.ValuationResultCurrentPrice is null or B.ValuationResultFuturePrice is null then '' else ',' end)
+(case when B.ValuationResultFuturePrice is null then '' else '{"Price":'+cast(B.ValuationResultFuturePrice as varchar(20))+',"PriceSourceType":"STANDARD_PRICE_FUTURE"}' end)
+']'
from [dbo].[RealEstateValuation] A
inner join [dbo].[RealEstateValuationOld] B on A.RealEstateValuationId=B.RealEstateValuationId;
