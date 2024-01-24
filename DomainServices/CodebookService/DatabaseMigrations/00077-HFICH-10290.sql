DROP TABLE IF EXISTS [dbo].[RealEstatePriceType];
GO

CREATE TABLE [dbo].[RealEstatePriceType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Code] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RealEstatePriceTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into [dbo].[RealEstatePriceType] (Id, [Name], Code) values
(1,'cena po místním šetření','PRICE_AFTER_LS'),
(2,'cena předběžná','FLAT_PRELIMINARY_PRICE'),
(3,'zástavní hodnota','PLEDGED_VALUE'),
(4,'cena minimální','STANDARD_PRICE_MIN'),
(5,'cena současná','STANDARD_PRICE'),
(6,'cena současná','STANDARD_PRICE_EXIST'),
(7,'cena budoucí','STANDARD_PRICE_FUTURE'),
(8,'cena současná ke dni ZOV','STANDARD_PRICE_ZOV'),
(9,'cena budoucí','LAND_PRICE_FUTURE'),
(10,'cena současná','Online');
GO

insert into [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) values ('RealEstatePriceTypes', 'SELECT * FROM [dbo].[RealEstatePriceType] ORDER BY Id ASC', 4);
GO
