--Old CustomersOnSa
UPDATE DataService SET DataServiceName = 'HouseholdMainService' WHERE DataServiceId = 9
--Old CustomersOnSaDetail
UPDATE DataService SET DataServiceName = 'HouseholdCodebtorService' WHERE DataServiceId = 10
--New ID for DocumentOnSa
UPDATE DataService SET DataServiceName = 'DocumentOnSa' WHERE DataServiceId = 11

UPDATE DataField SET DataServiceId = 9 WHERE DataServiceId = 10
UPDATE DataField SET DataServiceId = 8 WHERE DataServiceId = 11
UPDATE DataField SET DataServiceId = 11 WHERE DataServiceId = 12

UPDATE DocumentSpecialDataField SET DataServiceId = 9 WHERE DataServiceId = 10
UPDATE DocumentSpecialDataField SET DataServiceId = 10 WHERE DataServiceId = 11

DELETE FROM DataService WHERE DataServiceId = 12

UPDATE DataField SET FieldPath = REPLACE(FieldPath, 'CustomerOnSaDebtor', 'HouseholdMain.Debtor') WHERE DataFieldId IN (163, 164, 165, 166, 167)
UPDATE DataField SET FieldPath = REPLACE(FieldPath, 'CustomerOnSaCodebtor', 'HouseholdMain.Codebtor') WHERE DataFieldId IN (168, 169, 170, 171, 172)

UPDATE DocumentSpecialDataField SET DataServiceId = 10 WHERE DocumentId = 5 AND AcroFieldName LIKE 'PodpisJmenoKlienta%'

SET IDENTITY_INSERT [dbo].[DataField] OFF 

--ZADOSTHU fixed missing DynamicStrings for paragraphs
SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (45, 168, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (46, 169, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (47, 170, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (48, 171, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (49, 172, 'jsem Americkou osobou.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (50, 173, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (51, 174, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (52, 175, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (53, 176, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (54, 177, 'jsem Americkou osobou.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (45, 'True', 163)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (46, 'True', 164)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (47, 'True', 165)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (48, 'True', 166)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (49, 'True', 167)

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (50, 'True', 168)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (51, 'True', 169)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (52, 'True', 170)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (53, 'True', 171)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (54, 'True', 172)


--ZADOSTHD paragraphs
SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (173, 10, 'HouseholdCodebtor.CustomerOnSa1.CustomerAdditionalData.HasRelationshipWithKB', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (174, 10, 'HouseholdCodebtor.CustomerOnSa1.CustomerAdditionalData.HasRelationshipWithKBEmployee', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (175, 10, 'HouseholdCodebtor.CustomerOnSa1.CustomerAdditionalData.IsPoliticallyExposed', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (176, 10, 'HouseholdCodebtor.CustomerOnSa1.CustomerAdditionalData.HasRelationshipWithCorporate', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (177, 10, 'HouseholdCodebtor.CustomerOnSa1.CustomerAdditionalData.IsUSPerso', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (178, 10, 'HouseholdCodebtor.CustomerOnSa2.CustomerAdditionalData.HasRelationshipWithKB', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (179, 10, 'HouseholdCodebtor.CustomerOnSa2.CustomerAdditionalData.HasRelationshipWithKBEmployee', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (180, 10, 'HouseholdCodebtor.CustomerOnSa2.CustomerAdditionalData.IsPoliticallyExposed', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (181, 10, 'HouseholdCodebtor.CustomerOnSa2.CustomerAdditionalData.HasRelationshipWithCorporate', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (182, 10, 'HouseholdCodebtor.CustomerOnSa2.CustomerAdditionalData.IsUSPerso', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (178, 5, '001', 173, 'JsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (179, 5, '001', 174, 'JsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (180, 5, '001', 175, 'JsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (181, 5, '001', 176, 'JsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (182, 5, '001', 177, 'JsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (183, 5, '001', 178, 'SpoluzadatelJsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (184, 5, '001', 179, 'SpoluzadatelJsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (185, 5, '001', 180, 'SpoluzadatelJsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (186, 5, '001', 181, 'SpoluzadatelJsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (187, 5, '001', 182, 'SpoluzadatelJsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (55, 178, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (56, 179, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (57, 180, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (58, 181, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (59, 182, 'jsem Americkou osobou.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (60, 183, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (61, 184, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (62, 185, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (63, 186, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (64, 187, 'jsem Americkou osobou.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (55, 'True', 173)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (56, 'True', 174)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (57, 'True', 175)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (58, 'True', 176)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (59, 'True', 177)

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (60, 'True', 178)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (61, 'True', 179)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (62, 'True', 180)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (63, 'True', 181)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (64, 'True', 182)

INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) 
VALUES (5, 'JsemNejsem5Odrazka', 10, 'Customer1.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')

INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) 
VALUES (5, 'SpoluzadatelJsemNejsem5Odrazka', 10, 'Customer2.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')

--New 3602 ZUSTAVSI + PRISTOUP
INSERT INTO Document VALUES (11, 'ZUSTAVSI')
INSERT INTO Document VALUES (12, 'PRISTOUP')

INSERT INTO DocumentDynamicInputParameter VALUES (11, '001', 2, 6, 1)
INSERT INTO DocumentDynamicInputParameter VALUES (12, '001', 2, 6, 1)

SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (183, 6, 'Mortgage.LoanAmount', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (184, 6, 'Mortgage.FixedRatePeriod', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (185, 6, 'Mortgage.LoanDueDate', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

--ZUSTAVSI
SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (188, 11, '001', 60, 'RegCislo', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (189, 11, '001', 183, 'VyseUveru', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (190, 11, '001', 184, 'DelkaFixace', '{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (191, 11, '001', 185, 'Splatnost', '{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (192, 11, '001', 117, 'PocetDetiDo10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (193, 11, '001', 118, 'PocetDetiNad10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (194, 11, '001', 119, 'NakladyBydleni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (195, 11, '001', 120, 'Pojisteni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (196, 11, '001', 121, 'Sporeni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (197, 11, '001', 122, 'OstatniVydaje', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (198, 11, '001', 60, 'ProhlaseniPristupujicich', 'Prohlašuji, že je mi známo, že původní dlužník s Bankou uzavřel smlouvu o úvěru reg. číslo {0} a že se zněním této smlouvy jsem byl původním dlužníkem seznámen.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (199, 11, '001', 173, 'JsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (200, 11, '001', 174, 'JsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (201, 11, '001', 175, 'JsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (202, 11, '001', 176, 'JsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (203, 11, '001', 177, 'JsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (204, 11, '001', 178, 'SpoluzadatelJsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (205, 11, '001', 179, 'SpoluzadatelJsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (206, 11, '001', 180, 'SpoluzadatelJsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (207, 11, '001', 181, 'SpoluzadatelJsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (208, 11, '001', 182, 'SpoluzadatelJsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (65, 199, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (66, 200, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (67, 201, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (68, 202, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (69, 203, 'jsem Americkou osobou.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (70, 204, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (71, 205, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (72, 206, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (73, 207, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (74, 208, 'jsem Americkou osobou.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (65, 'True', 173)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (66, 'True', 174)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (67, 'True', 175)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (68, 'True', 176)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (69, 'True', 177)

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (70, 'True', 178)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (71, 'True', 179)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (72, 'True', 180)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (73, 'True', 181)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (74, 'True', 182)

INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'DatumNarozeni', 10, N'Customer1.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'DokladTotoznosti', 10, N'Customer1.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'JmenoPrijmeni', 10, N'Customer1.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'KontaktniAdresa', 10, N'Customer1.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'MesSplatkyHypUvery', 10, N'Customer1Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'MesSplatkySpotrUvery', 10, N'Customer1Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'MesSplatkyStavSpor', 10, N'Customer1Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'NesplJistinaHypUvery', 10, N'Customer1Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'NesplJistinaSpotrUvery', 10, N'Customer1Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'NesplJistinaStavSpor', 10, N'Customer1Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'NovyLimitDebet', 10, N'Customer1Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'NovyLimitKreditniKarty', 10, N'Customer1Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'Ostatni', 10, N'Customer1Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'PodpisJmenoKlienta', 10, N'Customer1.SignerName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'PodpisJmenoKlienta2', 10, N'Customer2.SignerName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'RodneCislo', 10, N'Customer1.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimDebet', 10, N'Customer1Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimKreditniKarty', 10, N'Customer1Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimPredHypUvery', 10, N'Customer1Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimPredSpotrUvery', 10, N'Customer1Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimPredStavSpor', 10, N'Customer1Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimUveremHypUvery', 10, N'Customer1Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimUveremSpotrUvery', 10, N'Customer1Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SplatimUveremStavSpor', 10, N'Customer1Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelDatumNarozeni', 10, N'Customer2.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelDokladTotoznosti', 10, N'Customer2.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelJmenoPrijmeni', 10, N'Customer2.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelKontaktniAdresa', 10, N'Customer2.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelMesSplatkyHypUvery', 10, N'Customer2Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelMesSplatkySpotrUvery', 10, N'Customer2Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelMesSplatkyStavSpor', 10, N'Customer2Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelNesplJistinaHypUvery', 10, N'Customer2Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelNesplJistinaSpotrUvery', 10, N'Customer2Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelNesplJistinaStavSpor', 10, N'Customer2Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelNovyLimitDebet', 10, N'Customer2Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelNovyLimitKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelOstatni', 10, N'Customer2Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelRodneCislo', 10, N'Customer2.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimDebet', 10, N'Customer2Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimKreditniKarty', 10, N'Customer2Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimPredHypUvery', 10, N'Customer2Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimPredSpotrUvery', 10, N'Customer2Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimPredStavSpor', 10, N'Customer2Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimUveremHypUvery', 10, N'Customer2Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimUveremSpotrUvery', 10, N'Customer2Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelSplatimUveremStavSpor', 10, N'Customer2Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelStav', 10, N'Customer2MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelTelEmail', 10, N'Customer2.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelTrvalyPobyt', 10, N'Customer2.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelUpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelVyseLimituDebet', 10, N'Customer2Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelVyseLimituKreditniKarty', 10, N'Customer2Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelVzdelani', 10, N'Customer2.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelZeZamestnani', 10, N'Customer2Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelZPodnikani', 10, N'Customer2Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelZPronajmu', 10, N'Customer2Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelZrusimDebet', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'SpoluzadatelZrusimKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'Stav', 10, N'Customer1MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'TelEmail', 10, N'Customer1.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'TrvalyPobyt', 10, N'Customer1.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'TypUveru', 6, N'LoanType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'UcelUveru', 6, N'LoanPurposes', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'UpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'VyseLimituDebet', 10, N'Customer1Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'VyseLimituKreditniKarty', 10, N'Customer1Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'Vzdelani', 10, N'Customer1.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'ZeZamestnani', 10, N'Customer1Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'ZPodnikani', 10, N'Customer1Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'ZPronajmu', 10, N'Customer1Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, N'ZrusimDebet', 10, N'Customer1Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, 'JsemNejsem5Odrazka', 10, 'Customer1.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (11, 'SpoluzadatelJsemNejsem5Odrazka', 10, 'Customer2.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')


INSERT INTO DocumentDataFieldVariant VALUES (188, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (188, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (189, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (189, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (190, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (190, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (191, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (191, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (192, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (192, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (193, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (193, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (194, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (194, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (195, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (195, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (196, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (196, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (197, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (197, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (198, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (198, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (199, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (199, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (200, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (200, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (201, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (201, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (202, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (202, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (203, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (203, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (204, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (205, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (206, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (207, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (208, 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'DatumNarozeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'DatumNarozeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'DokladTotoznosti', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'DokladTotoznosti', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'JmenoPrijmeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'JmenoPrijmeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'KontaktniAdresa', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'KontaktniAdresa', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkyHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkyHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkySpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkySpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkyStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'MesSplatkyStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NesplJistinaStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NovyLimitDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NovyLimitDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NovyLimitKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'NovyLimitKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Ostatni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Ostatni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'PodpisJmenoKlienta', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'PodpisJmenoKlienta', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'PodpisJmenoKlienta2', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'RodneCislo', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'RodneCislo', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimPredStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SplatimUveremStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Stav', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Stav', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TelEmail', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TelEmail', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TrvalyPobyt', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TrvalyPobyt', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TypUveru', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'TypUveru', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'UcelUveru', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'UcelUveru', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'UpravaSpolecnehoJmeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'UpravaSpolecnehoJmeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'VyseLimituDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'VyseLimituDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'VyseLimituKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'VyseLimituKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Vzdelani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'Vzdelani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZeZamestnani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZeZamestnani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZPodnikani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZPodnikani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZPronajmu', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZPronajmu', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZrusimDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'ZrusimDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'JsemNejsem5Odrazka', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'JsemNejsem5Odrazka', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelDatumNarozeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelDokladTotoznosti', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelJmenoPrijmeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelKontaktniAdresa', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelMesSplatkyHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelMesSplatkySpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelMesSplatkyStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelNesplJistinaHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelNesplJistinaSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelNesplJistinaStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelNovyLimitDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelNovyLimitKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelOstatni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelRodneCislo', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimPredHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimPredSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimPredStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimUveremHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimUveremSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelSplatimUveremStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelStav', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelTelEmail', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelTrvalyPobyt', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelUpravaSpolecnehoJmeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelVyseLimituDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelVyseLimituKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelVzdelani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelZeZamestnani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelZPodnikani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelZPronajmu', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelZrusimDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelZrusimKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (11, 'SpoluzadatelJsemNejsem5Odrazka', 'B')


--PRISTOUP
SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (209, 12, '001', 60, 'RegCislo', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (210, 12, '001', 183, 'VyseUveru', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (211, 12, '001', 184, 'DelkaFixace', '{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (212, 12, '001', 185, 'Splatnost', '{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (213, 12, '001', 117, 'PocetDetiDo10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (214, 12, '001', 118, 'PocetDetiNad10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (215, 12, '001', 119, 'NakladyBydleni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (216, 12, '001', 120, 'Pojisteni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (217, 12, '001', 121, 'Sporeni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (218, 12, '001', 122, 'OstatniVydaje', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (219, 12,  '001', 173, 'JsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (220, 12,  '001', 174, 'JsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (221, 12, '001', 175, 'JsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (222, 12, '001', 176, 'JsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (223, 12, '001', 177, 'JsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (224, 12, '001', 178, 'SpoluzadatelJsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (225, 12, '001', 179, 'SpoluzadatelJsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (226, 12, '001', 180, 'SpoluzadatelJsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (227, 12, '001', 181, 'SpoluzadatelJsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (228, 12, '001', 182, 'SpoluzadatelJsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (75, 219, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (76, 220, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (77, 221, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (78, 222, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (79, 223, 'jsem Americkou osobou.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (80, 224, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (81, 225, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (82, 226, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (83, 227, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (84, 228, 'jsem Americkou osobou.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (75, 'True', 173)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (76, 'True', 174)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (77, 'True', 175)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (78, 'True', 176)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (79, 'True', 177)

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (80, 'True', 178)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (81, 'True', 179)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (82, 'True', 180)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (83, 'True', 181)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (84, 'True', 182)


INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'DatumNarozeni', 10, N'Customer1.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'DokladTotoznosti', 10, N'Customer1.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'JmenoPrijmeni', 10, N'Customer1.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'KontaktniAdresa', 10, N'Customer1.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'MesSplatkyHypUvery', 10, N'Customer1Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'MesSplatkySpotrUvery', 10, N'Customer1Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'MesSplatkyStavSpor', 10, N'Customer1Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'NesplJistinaHypUvery', 10, N'Customer1Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'NesplJistinaSpotrUvery', 10, N'Customer1Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'NesplJistinaStavSpor', 10, N'Customer1Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'NovyLimitDebet', 10, N'Customer1Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'NovyLimitKreditniKarty', 10, N'Customer1Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'Ostatni', 10, N'Customer1Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'PodpisJmenoKlienta', 10, N'Customer1.SignerName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'PodpisJmenoKlienta2', 10, N'Customer2.SignerName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'RodneCislo', 10, N'Customer1.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimDebet', 10, N'Customer1Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimKreditniKarty', 10, N'Customer1Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimPredHypUvery', 10, N'Customer1Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimPredSpotrUvery', 10, N'Customer1Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimPredStavSpor', 10, N'Customer1Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimUveremHypUvery', 10, N'Customer1Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimUveremSpotrUvery', 10, N'Customer1Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SplatimUveremStavSpor', 10, N'Customer1Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelDatumNarozeni', 10, N'Customer2.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelDokladTotoznosti', 10, N'Customer2.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelJmenoPrijmeni', 10, N'Customer2.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelKontaktniAdresa', 10, N'Customer2.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelMesSplatkyHypUvery', 10, N'Customer2Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelMesSplatkySpotrUvery', 10, N'Customer2Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelMesSplatkyStavSpor', 10, N'Customer2Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelNesplJistinaHypUvery', 10, N'Customer2Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelNesplJistinaSpotrUvery', 10, N'Customer2Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelNesplJistinaStavSpor', 10, N'Customer2Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelNovyLimitDebet', 10, N'Customer2Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelNovyLimitKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelOstatni', 10, N'Customer2Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelRodneCislo', 10, N'Customer2.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimDebet', 10, N'Customer2Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimKreditniKarty', 10, N'Customer2Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimPredHypUvery', 10, N'Customer2Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimPredSpotrUvery', 10, N'Customer2Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimPredStavSpor', 10, N'Customer2Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimUveremHypUvery', 10, N'Customer2Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimUveremSpotrUvery', 10, N'Customer2Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelSplatimUveremStavSpor', 10, N'Customer2Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelStav', 10, N'Customer2MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelTelEmail', 10, N'Customer2.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelTrvalyPobyt', 10, N'Customer2.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelUpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelVyseLimituDebet', 10, N'Customer2Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelVyseLimituKreditniKarty', 10, N'Customer2Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelVzdelani', 10, N'Customer2.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelZeZamestnani', 10, N'Customer2Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelZPodnikani', 10, N'Customer2Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelZPronajmu', 10, N'Customer2Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelZrusimDebet', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'SpoluzadatelZrusimKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'Stav', 10, N'Customer1MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'TelEmail', 10, N'Customer1.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'TrvalyPobyt', 10, N'Customer1.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'TypUveru', 6, N'LoanType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'UcelUveru', 6, N'LoanPurposes', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'UpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'VyseLimituDebet', 10, N'Customer1Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'VyseLimituKreditniKarty', 10, N'Customer1Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'Vzdelani', 10, N'Customer1.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'ZeZamestnani', 10, N'Customer1Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'ZPodnikani', 10, N'Customer1Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'ZPronajmu', 10, N'Customer1Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, N'ZrusimDebet', 10, N'Customer1Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, 'JsemNejsem5Odrazka', 10, 'Customer1.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (12, 'SpoluzadatelJsemNejsem5Odrazka', 10, 'Customer2.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')


INSERT INTO DocumentDataFieldVariant VALUES (209, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (209, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (210, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (210, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (211, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (211, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (212, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (212, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (213, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (213, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (214, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (214, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (215, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (215, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (216, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (216, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (217, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (217, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (218, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (218, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (219, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (219, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (220, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (220, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (221, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (221, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (222, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (222, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (223, 'A')
INSERT INTO DocumentDataFieldVariant VALUES (223, 'B')

INSERT INTO DocumentDataFieldVariant VALUES (224, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (225, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (226, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (227, 'B')
INSERT INTO DocumentDataFieldVariant VALUES (228, 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'DatumNarozeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'DatumNarozeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'DokladTotoznosti', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'DokladTotoznosti', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'JmenoPrijmeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'JmenoPrijmeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'KontaktniAdresa', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'KontaktniAdresa', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkyHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkyHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkySpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkySpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkyStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'MesSplatkyStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NesplJistinaStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NovyLimitDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NovyLimitDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NovyLimitKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'NovyLimitKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Ostatni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Ostatni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'PodpisJmenoKlienta', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'PodpisJmenoKlienta', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'PodpisJmenoKlienta2', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'RodneCislo', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'RodneCislo', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimPredStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremHypUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremHypUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremSpotrUvery', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremSpotrUvery', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremStavSpor', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SplatimUveremStavSpor', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Stav', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Stav', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TelEmail', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TelEmail', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TrvalyPobyt', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TrvalyPobyt', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TypUveru', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'TypUveru', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'UcelUveru', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'UcelUveru', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'UpravaSpolecnehoJmeni', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'UpravaSpolecnehoJmeni', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'VyseLimituDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'VyseLimituDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'VyseLimituKreditniKarty', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'VyseLimituKreditniKarty', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Vzdelani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'Vzdelani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZeZamestnani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZeZamestnani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZPodnikani', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZPodnikani', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZPronajmu', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZPronajmu', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZrusimDebet', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'ZrusimDebet', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'JsemNejsem5Odrazka', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'JsemNejsem5Odrazka', 'B')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelDatumNarozeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelDokladTotoznosti', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelJmenoPrijmeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelKontaktniAdresa', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelMesSplatkyHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelMesSplatkySpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelMesSplatkyStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelNesplJistinaHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelNesplJistinaSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelNesplJistinaStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelNovyLimitDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelNovyLimitKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelOstatni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelRodneCislo', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimPredHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimPredSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimPredStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimUveremHypUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimUveremSpotrUvery', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelSplatimUveremStavSpor', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelStav', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelTelEmail', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelTrvalyPobyt', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelUpravaSpolecnehoJmeni', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelVyseLimituDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelVyseLimituKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelVzdelani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelZeZamestnani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelZPodnikani', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelZPronajmu', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelZrusimDebet', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelZrusimKreditniKarty', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (12, 'SpoluzadatelJsemNejsem5Odrazka', 'B')