--HUBN fix
DELETE FROM DocumentDataField WHERE DocumentDataFieldId = 150
DELETE FROM DocumentDataField WHERE DocumentDataFieldId = 151

--Offer
UPDATE DynamicStringFormat SET Format = 'doložení průkazu energetické náročnosti ve třídě A nebo B k nemovitosti financované úvěrem nebo nemovitosti určené k zajištění úvěru' WHERE DynamicStringFormatId = 10

--Calculation
UPDATE DocumentDataField SET StringFormat = 'doložení průkazu energetické náročnosti ve třídě A nebo B k nemovitosti financované úvěrem nebo nemovitosti určené k zajištění úvěru' WHERE DocumentDataFieldId = 33
UPDATE DynamicStringFormat SET Format = '--' WHERE DynamicStringFormatId = 17

--PaymentSchedule
UPDATE DocumentDataField SET DataFieldId = 18, StringFormat = NULL WHERE DocumentDataFieldId = 158

--Common
UPDATE DocumentDataField SET StringFormat = 'Tuto žádost přijal {0}.' WHERE AcroFieldName = 'ZadostPrijal'

--GeneralChange
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 8 AND AcroFieldName = 'TypNemovitosti'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 8 AND AcroFieldName = 'UcelPorizeni'

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 8 AND AcroFieldName = 'NoveCisloUctu'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 8 AND AcroFieldName = 'MajitelUctu'

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (187, 1, 'SalesArrangement.GeneralChange.PaymentDay.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (188, 1, 'SalesArrangement.GeneralChange.LoanPurpose.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (189, 1, 'SalesArrangement.GeneralChange.LoanPaymentAmount.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (190, 1, 'SalesArrangement.GeneralChange.DueDate.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (191, 1, 'SalesArrangement.GeneralChange.DrawingAndOtherConditions.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (192, 1, 'SalesArrangement.GeneralChange.CommentToChangeRequest.IsActive', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (95, 138, '--', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (96, 139, '--', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (97, 140, '--', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (98, 141, '--', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (99, 143, '--', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (100, 144, '--', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

UPDATE DynamicStringFormat SET [Priority] = 2 WHERE DynamicStringFormatId = 40

INSERT INTO DynamicStringFormatCondition VALUES (95, 'False', 187)
INSERT INTO DynamicStringFormatCondition VALUES (96, 'False', 188)
INSERT INTO DynamicStringFormatCondition VALUES (97, 'False', 189)
INSERT INTO DynamicStringFormatCondition VALUES (98, 'False', 190)
INSERT INTO DynamicStringFormatCondition VALUES (99, 'False', 191)
INSERT INTO DynamicStringFormatCondition VALUES (100, 'False', 192)

UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 139
UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 138
UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 140
UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 141
UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 143
UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 144

DELETE FROM DocumentDataField WHERE DocumentDataFieldId = 142

INSERT INTO DocumentSpecialDataField VALUES (8, 'ZmenaLhutyCerpaniLabel', 1, 'ExtensionDrawingDateLabel', NULL, NULL, 'Změna lhůty čerpání')
INSERT INTO DocumentSpecialDataField VALUES (8, 'ZmenaLhutyCerpaniL', 1, 'ExtensionDrawingDate', 'o {0} měsíců', NULL, '--')

UPDATE DocumentSpecialDataField SET FieldPath = 'SignerName' WHERE DocumentId = 8 AND AcroFieldName = 'PodpisJmenoKlienta'