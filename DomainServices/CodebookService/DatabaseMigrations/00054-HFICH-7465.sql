CREATE TABLE [dbo].[LoanPurposeExtension](
	[AcvId] [varchar](30) NOT NULL,
	[AcvIdPriority] [int] NOT NULL
) ON [PRIMARY]
GO

INSERT [dbo].[LoanPurposeExtension] ([AcvId], [AcvIdPriority]) VALUES (N'JINY', 25)
GO
INSERT [dbo].[LoanPurposeExtension] ([AcvId], [AcvIdPriority]) VALUES (N'VYST', 20)
GO
INSERT [dbo].[LoanPurposeExtension] ([AcvId], [AcvIdPriority]) VALUES (N'REKO', 15)
GO
INSERT [dbo].[LoanPurposeExtension] ([AcvId], [AcvIdPriority]) VALUES (N'KOUP', 10)
GO
INSERT [dbo].[LoanPurposeExtension] ([AcvId], [AcvIdPriority]) VALUES (N'KONS', 5)
GO

UPDATE [dbo].[SqlQuery] SET SqlQueryId='LoanPurposes1' WHERE SqlQueryId='LoanPurposes';
INSERT INTO SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('LoanPurposes2', 'SELECT AcvId, AcvIdPriority FROM LoanPurposeExtension', 4);
