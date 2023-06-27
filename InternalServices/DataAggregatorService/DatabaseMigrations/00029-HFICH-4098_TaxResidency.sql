INSERT INTO Document VALUES (13, 'DANRESID')

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (195, 5, 'Customer.NaturalPerson.TaxResidence.ResidenceCountries[].Tin', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (196, 5, 'Customer.NaturalPerson.TaxResidence.ResidenceCountries[].TinMissingReasonDescription', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (197, 5, 'CustomerOnSA.CustomerAdditionalData.IsUSPerson', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (198, 5, 'Customer.NaturalPerson.TaxResidence.ResidenceCountries[].CountryId', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (250, 13, '001', 29, 'RodneCisloNazev', 'Rodné číslo:', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (251, 13, '001', 29, 'RodneCislo', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (252, 13, '001', 30, 'DatumNarozeniNazev', '', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (253, 13, '001', 30, 'DatumNarozeni', '', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (254, 13, '001', 195, 'DanoveCislo', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (255, 13, '001', 196, 'DuvodNeposkytnuti', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (256, 13, '001', 29, 'ObcanUSA', 'Potvrzuji, že nejsem občanem Spojených států amerických.', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (257, 13, '001', 29, 'RezidentUSA', 'Potvrzuji, že nejsem rezidentem Spojených států amerických pro daňové účely.', NULL, NULL)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (103, 250, '', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (104, 252, 'Datum narození:', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (105, 253, '{0}', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (106, 256, 'Potvrzuji, že jsem občanem Spojených států amerických.', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (107, 257, 'Potvrzuji, že jsem rezidentem Spojených států amerických pro daňové účely.', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT INTO DynamicStringFormatCondition VALUES (103, '', 29)
INSERT INTO DynamicStringFormatCondition VALUES (104, '', 29)
INSERT INTO DynamicStringFormatCondition VALUES (105, '', 29)
INSERT INTO DynamicStringFormatCondition VALUES (106, 'True', 197)
INSERT INTO DynamicStringFormatCondition VALUES (107, '226', 198)

INSERT INTO DocumentSpecialDataField VALUES (13, 'JmenoPrijmeni', 5, 'FullName', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (13, 'TrvalyPobyt', 5, 'PermanentAddress', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (13, 'KorespondencniAdresa', 5, 'CorrespondenceAddress', NULL, NULL, 'adresa trvalého pobytu')
INSERT INTO DocumentSpecialDataField VALUES (13, 'MistoNarozeni', 5, 'BirthPlace', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (13, 'ZemeDanoveRezidence', 5, 'TaxResidencyCountries', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (13, 'PoskytujeTIN', 5, 'TaxResidencyCountriesTinMandatory', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (13, 'PodpisJmenoKlienta', 5, 'SignerName', NULL, NULL, NULL)