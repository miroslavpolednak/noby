INSERT INTO Document VALUES (16, 'ZADOSTHD_SERVICE')

INSERT INTO DocumentDynamicInputParameter VALUES (16, '001', 2, 6, 1)

SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (186, 6, 'Mortgage.ExpectedDateOfDrawing', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

--ZADOSTHD Service
SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (229, 16, N'001', 60, N'RegCislo', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (230, 16, N'001', 183, N'VyseUveru', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (231, 16, N'001', 184, N'DelkaFixace', N'{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (232, 16, N'001', 185, N'Splatnost', N'{0:MonthsToYears}', NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (233, 16, N'001', 186, N'ZahajeniCerpani', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (234, 16, N'001', 117, N'PocetDetiDo10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (235, 16, N'001', 118, N'PocetDetiNad10', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (236, 16, N'001', 119, N'NakladyBydleni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (237, 16, N'001', 120, N'Pojisteni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (238, 16, N'001', 121, N'Sporeni', NULL, NULL, NULL)
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) VALUES (239, 16, N'001', 122, N'OstatniVydaje', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (240, 16, '001', 173, 'JsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (241, 16, '001', 174, 'JsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (242, 16, '001', 175, 'JsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (243, 16, '001', 176, 'JsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (244, 16, '001', 177, 'JsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (245, 16, '001', 178, 'SpoluzadatelJsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (246, 16, '001', 179, 'SpoluzadatelJsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (247, 16, '001', 180, 'SpoluzadatelJsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (248, 16, '001', 181, 'SpoluzadatelJsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací.', NULL, 4)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (249, 16, '001', 182, 'SpoluzadatelJsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (85, 240, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (86, 241, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (87, 242, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (88, 243, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (89, 244, 'jsem Americkou osobou.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (90, 245, 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (91, 246, 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (92, 247, 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (93, 248, 'jsem propojen s obchodní korporací.', 1)

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) 
VALUES (94, 249, 'jsem Americkou osobou.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (85, 'True', 173)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (86, 'True', 174)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (87, 'True', 175)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (88, 'True', 176)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (89, 'True', 177)

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (90, 'True', 178)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (91, 'True', 179)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (92, 'True', 180)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (93, 'True', 181)
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (94, 'True', 182)

INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'DatumNarozeni', 10, N'Customer1.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'DokladTotoznosti', 10, N'Customer1.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'JmenoPrijmeni', 10, N'Customer1.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'JsemNejsem5Odrazka', 10, N'Customer1.CzechResidence', N'jsem českým daňovým residentem,', 4, N'nejsem českým daňovým residentem,')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'KontaktniAdresa', 10, N'Customer1.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'MesSplatkyHypUvery', 10, N'Customer1Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'MesSplatkySpotrUvery', 10, N'Customer1Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'MesSplatkyStavSpor', 10, N'Customer1Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'NesplJistinaHypUvery', 10, N'Customer1Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'NesplJistinaSpotrUvery', 10, N'Customer1Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'NesplJistinaStavSpor', 10, N'Customer1Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'NovyLimitDebet', 10, N'Customer1Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'NovyLimitKreditniKarty', 10, N'Customer1Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'Ostatni', 10, N'Customer1Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'PodpisJmenoKlienta', 10, N'Customer1.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'PodpisJmenoKlienta2', 10, N'Customer2.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'RodneCislo', 10, N'Customer1.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimDebet', 10, N'Customer1Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimKreditniKarty', 10, N'Customer1Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimPredHypUvery', 10, N'Customer1Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimPredSpotrUvery', 10, N'Customer1Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimPredStavSpor', 10, N'Customer1Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimUveremHypUvery', 10, N'Customer1Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimUveremSpotrUvery', 10, N'Customer1Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SplatimUveremStavSpor', 10, N'Customer1Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelDatumNarozeni', 10, N'Customer2.DateOfBirth', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelDokladTotoznosti', 10, N'Customer2.IdentificationType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelJmenoPrijmeni', 10, N'Customer2.FullName', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelJsemNejsem5Odrazka', 10, N'Customer2.CzechResidence', N'jsem českým daňovým residentem,', 4, N'nejsem českým daňovým residentem,')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelKontaktniAdresa', 10, N'Customer2.ContactAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelMesSplatkyHypUvery', 10, N'Customer2Obligation.ObligationMLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelMesSplatkySpotrUvery', 10, N'Customer2Obligation.ObligationCLInstallment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelMesSplatkyStavSpor', 10, N'Customer2Obligation.ObligationML2Installment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelNesplJistinaHypUvery', 10, N'Customer2Obligation.ObligationMLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelNesplJistinaSpotrUvery', 10, N'Customer2Obligation.ObligationCLLoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelNesplJistinaStavSpor', 10, N'Customer2Obligation.ObligationML2LoanPrincipal', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelNovyLimitDebet', 10, N'Customer2Obligation.CreditCardCorrectionAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelNovyLimitKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelOstatni', 10, N'Customer2Income.IncomeOther', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelRodneCislo', 10, N'Customer2.BirthNumber', NULL, NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimDebet', 10, N'Customer2Obligation.ObligationADAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimKreditniKarty', 10, N'Customer2Obligation.ObligationCCAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimPredHypUvery', 10, N'Customer2Obligation.ObligationMLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimPredSpotrUvery', 10, N'Customer2Obligation.ObligationCLSumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimPredStavSpor', 10, N'Customer2Obligation.ObligationML2SumWithCorrection', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimUveremHypUvery', 10, N'Customer2Obligation.ObligationMLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimUveremSpotrUvery', 10, N'Customer2Obligation.ObligationCLAmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelSplatimUveremStavSpor', 10, N'Customer2Obligation.ObligationML2AmountConsolidated', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelStav', 10, N'Customer2MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelTelEmail', 10, N'Customer2.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelTrvalyPobyt', 10, N'Customer2.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelUpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelVyseLimituDebet', 10, N'Customer2Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelVyseLimituKreditniKarty', 10, N'Customer2Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelVzdelani', 10, N'Customer2.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelZeZamestnani', 10, N'Customer2Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelZPodnikani', 10, N'Customer2Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelZPronajmu', 10, N'Customer2Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelZrusimDebet', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'SpoluzadatelZrusimKreditniKarty', 10, N'Customer2Obligation.CreditCardCorrectionConsolidatedCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'Stav', 10, N'Customer1MaritalStatus', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'TelEmail', 10, N'Customer1.Contacts', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'TrvalyPobyt', 10, N'Customer1.PermanentAddress', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'TypUveru', 6, N'LoanType', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'UcelUveru', 6, N'LoanPurposes', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'UpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'VyseLimituDebet', 10, N'Customer1Obligation.CreditCardLimitAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'VyseLimituKreditniKarty', 10, N'Customer1Obligation.CreditCardLimitCC', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'Vzdelani', 10, N'Customer1.EducationLevel', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'ZeZamestnani', 10, N'Customer1Income.IncomeEmployment', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'ZPodnikani', 10, N'Customer1Income.IncomeEnterprise', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'ZPronajmu', 10, N'Customer1Income.IncomeRent', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'ZrusimDebet', 10, N'Customer1Obligation.CreditCardCorrectionConsolidatedAD', N'{0:CustomCurrency}', NULL, N'--')
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'ZrusimKreditniKarty', 10, N'Customer1Obligation.CreditCardCorrectionConsolidatedCC', N'{0:CustomCurrency}', NULL, N'--')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (229, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (229, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (229, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (229, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (230, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (230, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (230, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (230, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (231, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (231, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (231, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (231, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (232, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (232, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (232, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (232, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (233, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (233, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (233, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (233, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (234, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (234, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (234, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (234, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (235, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (235, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (235, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (235, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (236, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (236, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (236, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (236, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (237, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (237, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (237, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (237, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (238, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (238, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (238, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (238, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (239, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (239, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (239, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (239, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (240, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (240, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (240, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (240, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (241, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (241, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (241, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (241, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (242, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (242, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (242, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (242, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (243, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (243, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (243, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (243, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (244, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (244, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (244, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (244, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (245, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (245, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (246, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (246, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (247, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (247, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (248, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (248, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (249, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (249, N'D')


INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DatumNarozeni', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DatumNarozeni', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DatumNarozeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DatumNarozeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DokladTotoznosti', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DokladTotoznosti', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DokladTotoznosti', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'DokladTotoznosti', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JmenoPrijmeni', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JmenoPrijmeni', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JmenoPrijmeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JmenoPrijmeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'KontaktniAdresa', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'KontaktniAdresa', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'KontaktniAdresa', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'KontaktniAdresa', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyHypUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyHypUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkySpotrUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkySpotrUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkySpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkySpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyStavSpor', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyStavSpor', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'MesSplatkyStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaHypUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaHypUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaSpotrUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaSpotrUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaStavSpor', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaStavSpor', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NesplJistinaStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitDebet', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitDebet', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitKreditniKarty', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitKreditniKarty', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'NovyLimitKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Ostatni', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Ostatni', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Ostatni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Ostatni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta2', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'PodpisJmenoKlienta2', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'RodneCislo', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'RodneCislo', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'RodneCislo', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'RodneCislo', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimDebet', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimDebet', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimKreditniKarty', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimKreditniKarty', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredHypUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredHypUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredSpotrUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredSpotrUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredStavSpor', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredStavSpor', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimPredStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremHypUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremHypUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremSpotrUvery', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremSpotrUvery', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremStavSpor', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremStavSpor', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SplatimUveremStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelDatumNarozeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelDatumNarozeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelDokladTotoznosti', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelDokladTotoznosti', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelJmenoPrijmeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelJmenoPrijmeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelKontaktniAdresa', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelKontaktniAdresa', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkyHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkyHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkySpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkySpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkyStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelMesSplatkyStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNesplJistinaStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNovyLimitDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNovyLimitDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNovyLimitKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelNovyLimitKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelOstatni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelOstatni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelRodneCislo', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelRodneCislo', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimPredStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremHypUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremHypUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremSpotrUvery', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremSpotrUvery', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremStavSpor', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelSplatimUveremStavSpor', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelStav', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelStav', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelTelEmail', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelTelEmail', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelTrvalyPobyt', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelTrvalyPobyt', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelUpravaSpolecnehoJmeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelUpravaSpolecnehoJmeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVyseLimituDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVyseLimituDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVyseLimituKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVyseLimituKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVzdelani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelVzdelani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZeZamestnani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZeZamestnani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZPodnikani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZPodnikani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZPronajmu', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZPronajmu', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZrusimDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZrusimDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZrusimKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelZrusimKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Stav', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Stav', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Stav', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Stav', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TelEmail', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TelEmail', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TelEmail', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TelEmail', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TrvalyPobyt', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TrvalyPobyt', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TrvalyPobyt', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TrvalyPobyt', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TypUveru', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TypUveru', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TypUveru', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'TypUveru', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UcelUveru', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UcelUveru', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UcelUveru', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UcelUveru', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UpravaSpolecnehoJmeni', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UpravaSpolecnehoJmeni', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UpravaSpolecnehoJmeni', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'UpravaSpolecnehoJmeni', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituDebet', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituDebet', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituKreditniKarty', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituKreditniKarty', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'VyseLimituKreditniKarty', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Vzdelani', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Vzdelani', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Vzdelani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Vzdelani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZeZamestnani', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZeZamestnani', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZeZamestnani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZeZamestnani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPodnikani', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPodnikani', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPodnikani', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPodnikani', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPronajmu', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPronajmu', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPronajmu', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZPronajmu', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimDebet', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimDebet', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimDebet', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimDebet', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimKreditniKarty', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimKreditniKarty', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimKreditniKarty', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'ZrusimKreditniKarty', N'D')


--Splatnost_Label + missing variants
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (5, N'Splatnost_Label', 0, N'LoanDurationText', NULL, NULL, NULL)
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat], [TextAlign], [DefaultTextIfNull]) VALUES (16, N'Splatnost_Label', 0, N'LoanDurationText', NULL, NULL, NULL)

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'Splatnost_Label', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'Splatnost_Label', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'Splatnost_Label', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'Splatnost_Label', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Splatnost_Label', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Splatnost_Label', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Splatnost_Label', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'Splatnost_Label', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'JsemNejsem5Odrazka', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'JsemNejsem5Odrazka', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'JsemNejsem5Odrazka', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'JsemNejsem5Odrazka', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JsemNejsem5Odrazka', N'A')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JsemNejsem5Odrazka', N'B')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JsemNejsem5Odrazka', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'JsemNejsem5Odrazka', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'SpoluzadatelJsemNejsem5Odrazka', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (5, N'SpoluzadatelJsemNejsem5Odrazka', N'D')

INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelJsemNejsem5Odrazka', N'C')
INSERT [dbo].[DocumentSpecialDataFieldVariant] ([DocumentId], [AcroFieldName], [DocumentVariant]) VALUES (16, N'SpoluzadatelJsemNejsem5Odrazka', N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (178, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (178, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (178, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (178, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (179, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (179, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (179, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (179, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (180, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (180, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (180, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (180, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (181, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (181, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (181, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (181, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (182, N'A')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (182, N'B')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (182, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (182, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (183, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (183, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (184, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (184, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (185, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (185, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (186, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (186, N'D')

INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (187, N'C')
INSERT [dbo].[DocumentDataFieldVariant] ([DocumentDataFieldId], [DocumentVariant]) VALUES (187, N'D')