DROP TABLE IF EXISTS [dbo].[AcvRealEstateType];
GO

CREATE TABLE [dbo].[AcvRealEstateType](
	[RealEstateTypeId] [int] NOT NULL,
	[RealEstateSubtypeId] [int] NOT NULL,
	[RealEstateStateId] [int] NULL,
	[AcvRealEstateTypeId] [varchar](2) NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 1, N'05')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 2, N'06')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 3, N'06')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 1, 4, N'06')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 1, N'07')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 2, N'08')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 3, N'08')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (1, 2, 4, N'08')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 1, N'01')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 2, N'02')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 3, N'02')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (2, 3, 4, N'02')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (4, 5, NULL, N'25')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 1, N'09')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 2, N'10')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 3, N'10')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (5, 6, 4, N'10')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 1, N'15')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 2, N'16')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 3, N'16')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 7, 4, N'16')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 1, N'17')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 2, N'18')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 3, N'18')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 8, 4, N'18')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 1, N'19')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 2, N'20')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 3, N'20')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 9, 4, N'20')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 1, N'21')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 2, N'22')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 3, N'22')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 10, 4, N'22')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 1, N'11')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 2, N'12')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 3, N'12')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 11, 4, N'12')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 1, N'13')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 2, N'14')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 3, N'14')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (6, 12, 4, N'14')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 13, NULL, N'43')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 14, NULL, N'44')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 15, NULL, N'45')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (7, 16, NULL, N'54')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 1, N'23')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 2, N'24')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 3, N'24')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 17, 4, N'24')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 1, N'30')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 2, N'31')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 3, N'31')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 18, 4, N'31')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 1, N'32')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 2, N'33')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 3, N'33')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 19, 4, N'33')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 1, N'34')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 2, N'35')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 3, N'35')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 20, 4, N'35')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 1, N'36')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 2, N'37')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 3, N'37')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 21, 4, N'37')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 1, N'38')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 2, N'39')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 3, N'39')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 22, 4, N'39')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 1, N'41')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 2, N'42')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 3, N'42')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 23, 4, N'42')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 1, N'49')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 2, N'57')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 3, N'57')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 24, 4, N'57')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 1, N'51')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 2, N'58')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 3, N'58')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 25, 4, N'58')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 1, N'52')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 2, N'59')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 3, N'59')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 26, 4, N'59')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 1, N'46')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 2, N'61')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 3, N'61')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 27, 4, N'61')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 1, N'48')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 2, N'56')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 3, N'56')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 28, 4, N'56')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 1, N'53')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 2, N'60')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 3, N'60')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 29, 4, N'60')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 1, N'62')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 2, N'63')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 3, N'63')
GO
INSERT [dbo].[AcvRealEstateType] ([RealEstateTypeId], [RealEstateSubtypeId], [RealEstateStateId], [AcvRealEstateTypeId]) VALUES (8, 30, 4, N'63')
GO
