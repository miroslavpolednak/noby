--HFICH-4445 Drawing fix
SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (199, 1, 'SalesArrangement.Drawing.RepaymentAccount.IsAccountNumberMissing', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

UPDATE DocumentDataField SET StringFormat = '' WHERE DocumentDataFieldId = 159
UPDATE DynamicStringFormat SET [Format] = 'Jsem si vědom/a toho, že pokud nejsem majitelem účtu uvedeného pro splácení úvěru, je před prvním Čerpáním nutné doložit souhlas majitele tohoto účtu.' WHERE DynamicStringFormatId = 35
UPDATE DynamicStringFormatCondition SET DataFieldId = 199, EqualToValue = 'True' WHERE DynamicStringFormatId = 35

--HFICH-6101 HUBN
UPDATE DocumentDataField SET StringFormat = '--', DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 149

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (200, 1, 'SalesArrangement.HUBN.LoanAmount.ChangeAgreedLoanAmount', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (201, 1, 'SalesArrangement.HUBN.LoanAmount.PreserveAgreedLoanDueDate', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (202, 1, 'SalesArrangement.HUBN.LoanAmount.PreserveAgreedLoanPaymentAmount', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (204, 1, 'SalesArrangement.HUBN.ExpectedDateOfDrawing.IsActive', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (205, 1, 'SalesArrangement.HUBN.CommentToChangeRequest.IsActive', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (108, 149, 'požadovaná výše {0:CustomCurrency}, při zachování původně sjednané splatnosti', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (109, 149, 'požadovaná výše {0:CustomCurrency}, při zachování původně sjednané výše splátky', 2)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (110, 154, '{0}', 1)
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (111, 155, '{0}', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT INTO DynamicStringFormatCondition VALUES (108, 'True', 200)
INSERT INTO DynamicStringFormatCondition VALUES (108, 'True', 201)

INSERT INTO DynamicStringFormatCondition VALUES (109, 'True', 200)
INSERT INTO DynamicStringFormatCondition VALUES (109, 'True', 202)

UPDATE DocumentDataField SET StringFormat = '--' WHERE DocumentDataFieldId = 154
INSERT INTO DynamicStringFormatCondition VALUES (110, 'True', 204)

DELETE FROM DocumentDataField WHERE DocumentDataFieldId = 153
INSERT INTO DocumentSpecialDataField VALUES (10, 'LhutaUkonceniCerpani', 1, 'DrawingDateToText', NULL, NULL, NULL)

UPDATE DocumentDataField SET StringFormat = '--', TextAlign = 4 WHERE DocumentDataFieldId = 155
INSERT INTO DynamicStringFormatCondition VALUES (111, 'True', 205)

UPDATE DocumentDataField SET StringFormat = 'Tuto žádost prijal {0}.' + CHAR(13) + CHAR(10) +'Přijetí této žádosti není její akceptací Komerční bankou, a.s.' WHERE DocumentDataFieldId = 165

--HFICH-4785 ODSTOUP
INSERT INTO Document VALUES (15, 'ODSTOUP')

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (258, 15, '001', 22, 'DatumPodpisu', NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

INSERT INTO DocumentSpecialDataField VALUES (15, 'JmenoPrijmeni', 5, 'FullName', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (15, 'Ulice', 5, 'Street', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (15, 'PscMesto', 5, 'City', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (15, 'TextOznameni', 5, 'AnnouncementText', NULL, 4, NULL)