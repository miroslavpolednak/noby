﻿UPDATE RealEstateSubtypes SET ShortName=N'Pozemek do 3000 m2 pro rezidenční výstavbu', [Name]=N'Pozemek do 3000 m2 pro výstavbu rodinných domů a jinou obytnou zástavbu jako jediný předmět zajištění (nemá upřesnění typu nemovitosti)' WHERE Id=5;
UPDATE RealEstateSubtypes SET ShortName=N'Výrobní areál do 5. hlavních objektů', [Name]=N'Výrobní, skladový nebo ostatní areál nejvýše s 5 hlavními objekty o max. 4 NP a celkové zastavěné max. 2 500 m2 a zároveň o celkové maximální ploše areálu 10 000 m2. (Za hlavní objekty se považují takové, které jsou využívány nebo mohou být využívány k provozním účelům, např. výrobě, skladování, obchodu, administrativě, apod.)' WHERE Id=21;
UPDATE RealEstateSubtypes SET ShortName=N'Výrobní areál s více než 5 hlavními objekty', [Name]=N'Výrobní, skladový nebo ostatní areál s více než 5 hlavními objekty a celkové zastavěné ploše nad 2500 m2 nebo o ploše areálu nad  10.000 m2.' WHERE Id=27;
INSERT INTO RealEstateSubtypes (Id, ShortName, [Name], RealEstateTypeId) VALUES (32,N'Skladový areál do 5. hlavních objektů',N'Skladový nebo ostatní areál nejvýše s 5 hlavními objekty o max. 4 NP a celkové zastavěné max. 2 500 m2 a zároveň o celkové maximální ploše areálu 10 000 m2. (Za hlavní objekty se považují takové, které jsou využívány nebo mohou být využívány k provozním účelům, např. výrobě, skladování, obchodu, administrativě, apod.)',8);
INSERT INTO RealEstateSubtypes (Id, ShortName, [Name], RealEstateTypeId) VALUES (33,N'Skladový areál s více než 5 hlavními objekty',N'Skladový nebo ostatní areál s více než 5 hlavními objekty a celkové zastavěné ploše nad 2500 m2 nebo o ploše areálu nad  10.000 m2.',8);
INSERT INTO RealEstateSubtypes (Id, ShortName, [Name], RealEstateTypeId) VALUES (34,N'Pozemek do 3000 m2 pro komerční výstavbu',N'Pozemek do 3000 m2 pro výstavbu komerčních, výrobních a skladových objektů jako jediný předmět zajištění (nemá upřesnění typu nemovitosti)',4);
INSERT INTO RealEstateSubtypes (Id, ShortName, [Name], RealEstateTypeId) VALUES (35,N'Zařízení kulturní, školská a tělovýchovy',N'Jednoduchý objekt občanské vybavenosti: kulturní a školská zařízení, objekty tělovýchovy',8);

