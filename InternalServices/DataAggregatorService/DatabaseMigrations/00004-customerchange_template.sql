INSERT INTO Document VALUES (9, 'ZAOZMDLU')

SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT INTO DataField VALUES (156, 1, 'SalesArrangement.CustomerChange.Agent.NewAgent', NULL)
INSERT INTO DataField VALUES (157, 1, 'SalesArrangement.CustomerChange.CommentToChangeRequest.GeneralComment', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON 

INSERT INTO DocumentDataField VALUES (163, 9, '001', 156, 'ZmocnenecProDorucovani', NULL, NULL)
INSERT INTO DocumentDataField VALUES (164, 9, '001', 157, 'DetailniPopisText', NULL, NULL)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

INSERT INTO DocumentSpecialDataField VALUES (9, 'UvolneniZDluhu', 1, 'ReleaseCustomers', NULL)
INSERT INTO DocumentSpecialDataField VALUES (9, 'PristoupeniKDluhu', 1, 'AddCustomers', NULL)
INSERT INTO DocumentSpecialDataField VALUES (9, 'NoveCisloUctu', 1, 'BankAccount', NULL)
INSERT INTO DocumentSpecialDataField VALUES (9, 'MajitelUctu', 1, 'BankAccountOwner', NULL)