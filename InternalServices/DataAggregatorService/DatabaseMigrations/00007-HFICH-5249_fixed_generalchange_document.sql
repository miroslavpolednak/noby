UPDATE DocumentDataField SET TextAlign = 4 WHERE DocumentDataFieldId = 143
UPDATE DocumentDataField SET TextAlign = 4 WHERE DocumentDataFieldId = 144

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (158, 1, 'SalesArrangement.GeneralChange.LoanPaymentAmount.ConnectionExtraordinaryPayment', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON

INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (40, 140, '{0:CustomCurrency}; v souvislosti s mimořádnou splátkou', 1)

SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF

INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (40, 'True', 158)

UPDATE DocumentDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentDataFieldId = 140