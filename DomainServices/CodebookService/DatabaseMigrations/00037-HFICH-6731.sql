INSERT INTO dbo.SqlQuery (SqlQueryId, SqlQueryText,DatabaseProvider) VALUES ('GetAcvRealEstateType', 'SELECT TOP 1 [AcvRealEstateTypeId] FROM [MpssIntegration].[dbo].[AcvRealEstateType] WHERE [RealEstateTypeId]=@RealEstateTypeId AND [RealEstateSubtypeId]=@RealEstateSubtypeId AND [RealEstateStateId]=@RealEstateStateId',4);
GO

DROP TABLE IF EXISTS [dbo].[AcvRealEstateType];
GO

CREATE TABLE [dbo].[AcvRealEstateType](
	[RealEstateTypeId] [int] NOT NULL,
	[RealEstateSubtypeId] [int] NOT NULL,
	[RealEstateStateId] [int] NOT NULL,
	[AcvRealEstateTypeId] [int] NOT NULL
) ON [PRIMARY]
GO

INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 1, 5)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 2, 6)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 3, 6)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 4, 6)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 1, 7)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 2, 8)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 3, 8)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 4, 8)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 1, 1)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 2, 2)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 3, 2)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 4, 2)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (4, 5, 0, 25)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 1, 9)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 2, 10)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 3, 10)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 4, 10)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 1, 15)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 2, 16)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 3, 16)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 4, 16)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 1, 17)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 2, 18)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 3, 18)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 4, 18)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 1, 19)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 2, 20)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 3, 20)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 4, 20)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 1, 21)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 2, 22)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 3, 22)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 4, 22)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 1, 11)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 2, 12)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 3, 12)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 4, 12)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 1, 13)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 2, 14)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 3, 14)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 4, 14)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 13, 0, 43)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 14, 0, 44)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 15, 0, 45)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 16, 0, 54)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 1, 23)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 2, 24)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 3, 24)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 4, 24)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 1, 30)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 2, 31)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 3, 31)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 4, 31)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 1, 32)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 2, 33)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 3, 33)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 4, 33)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 1, 34)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 2, 35)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 3, 35)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 4, 35)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 1, 36)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 2, 37)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 3, 37)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 4, 37)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 1, 38)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 2, 39)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 3, 39)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 4, 39)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 1, 41)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 2, 42)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 3, 42)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 4, 42)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 1, 49)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 2, 57)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 3, 57)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 4, 57)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 1, 51)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 2, 58)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 3, 58)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 4, 58)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 1, 52)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 2, 59)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 3, 59)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 4, 59)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 1, 46)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 2, 61)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 3, 61)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 4, 61)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 1, 48)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 2, 56)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 3, 56)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 4, 56)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 1, 53)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 2, 60)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 3, 60)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 4, 60)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 1, 62)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 2, 63)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 3, 63)
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 4, 63)
GO
