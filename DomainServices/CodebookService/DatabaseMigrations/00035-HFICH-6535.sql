CREATE TABLE [dbo].[RealEstateState](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RealEstateState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RealEstateStates', N'SELECT Id, Name FROM dbo.RealEstateState ORDER BY Id', 4)
INSERT INTO [dbo].[RealEstateState] (Id, [Name]) VALUES
	(0,N'Unknown'),
	(1,N'Dokončená'),
	(2,N'V rekonstrukci'),
	(3,N'Projekt'),
	(4,N'Výstavba');