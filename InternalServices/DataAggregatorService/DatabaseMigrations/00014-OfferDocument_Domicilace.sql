UPDATE DynamicStringFormat SET Format = '--' WHERE DynamicStringFormatId = 1
UPDATE DynamicStringFormat SET Format = 'směřování Vašich příjmů na bankovní účet vedený u nás, a to alespoň ve výši 1,5 násobku výše splátky úvěru' WHERE DynamicStringFormatId = 2

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (41, 10, 'směřování Vašich příjmů na bankovní účet vedený u nás, a to alespoň ve výši 1,5 násobku výše splátky úvěru', 3)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (42, 10, 'ne', 4)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

DELETE FROM DynamicStringFormatCondition WHERE DynamicStringFormatId IN (1, 2)

INSERT INTO DynamicStringFormatCondition VALUES (1, '4', 69)
INSERT INTO DynamicStringFormatCondition VALUES (2, '2', 69)
INSERT INTO DynamicStringFormatCondition VALUES (41, '3', 69)
INSERT INTO DynamicStringFormatCondition VALUES (42, '1', 75)