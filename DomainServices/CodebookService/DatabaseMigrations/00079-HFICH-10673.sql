drop table if exists [dbo].[SignatureTypeDetail]
go
CREATE TABLE [dbo].[SignatureTypeDetail](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[IsRetentionDefault] [bit] NOT NULL,
	[IsRetentionAvailable] [bit] NOT NULL,
	[IsHedgeDefault] [bit] NOT NULL,
	[IsHedgeAvailable] [bit] NOT NULL,
	[IsIndividualDefault] [bit] NOT NULL,
	[IsIndividualAvailable] [bit] NOT NULL,
 CONSTRAINT [PK_SignatureTypeDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
insert into [dbo].[SqlQuery] (SqlQueryId, SqlQueryText, DatabaseProvider) values ('SignatureTypeDetails','SELECT * FROM dbo.SignatureTypeDetail',4);
go
INSERT [dbo].[SignatureTypeDetail] ([Id], [Name], [IsRetentionDefault], [IsRetentionAvailable], [IsHedgeDefault], [IsHedgeAvailable], [IsIndividualDefault], [IsIndividualAvailable]) VALUES (1, N'fyzicky poštou', 0, 1, 0, 1, 0, 1)
GO
INSERT [dbo].[SignatureTypeDetail] ([Id], [Name], [IsRetentionDefault], [IsRetentionAvailable], [IsHedgeDefault], [IsHedgeAvailable], [IsIndividualDefault], [IsIndividualAvailable]) VALUES (2, N'moje banka', 0, 0, 0, 0, 0, 1)
GO
INSERT [dbo].[SignatureTypeDetail] ([Id], [Name], [IsRetentionDefault], [IsRetentionAvailable], [IsHedgeDefault], [IsHedgeAvailable], [IsIndividualDefault], [IsIndividualAvailable]) VALUES (3, N'elektronicky', 0, 1, 0, 1, 0, 0)
GO
INSERT [dbo].[SignatureTypeDetail] ([Id], [Name], [IsRetentionDefault], [IsRetentionAvailable], [IsHedgeDefault], [IsHedgeAvailable], [IsIndividualDefault], [IsIndividualAvailable]) VALUES (4, N'fyzicky na pobočce', 1, 1, 1, 1, 1, 1)
GO
