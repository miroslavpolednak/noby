update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'K vyřízení' where Id=1;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Provozní podpora' where Id=2;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Odesláno' where Id=3;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Dokončeno' where Id=4;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Zrušeno' where Id=5;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Neoceněno' where Id=6;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Rozpracováno' where Id=7;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Probíhá ocenění' where Id=8;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Dožádání' where Id=9;
update [dbo].[WorkflowTaskStatesNoby] set [Name]=N'Doplnění dokumentů' where Id=10;
insert into WorkflowTaskStatesNoby (Id, [Name], [Filter], Indicator) values (11,N'Schváleno',2,3), (12,N'Zamítnuto',2,5);

update [dbo].[RealEstatePriceType] set [Name]=N'Cena po místním šetření' where Id=1;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena předběžná' where Id=2;
update [dbo].[RealEstatePriceType] set [Name]=N'Zástavní hodnota' where Id=3;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena minimální' where Id=4;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena současná' where Id=5;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena současná' where Id=6;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena budoucí' where Id=7;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena současná ke dni ZOV' where Id=8;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena budoucí' where Id=9;
update [dbo].[RealEstatePriceType] set [Name]=N'Cena současná' where Id=10;