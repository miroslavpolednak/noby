DROP TABLE [dbo].[WorkflowTaskStatesNoby];
GO

CREATE TABLE [dbo].[WorkflowTaskStatesNoby](
	[Id] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Filter] int NOT NULL,
	[Indicator] int NOT NULL,
 CONSTRAINT [PK_WorkflowTaskStatesNoby] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (1, N'K VYŘÍZENÍ', 1, 1)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (2, N'PROVOZNÍ PODPORA', 1, 1)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (3, N'ODESLÁNO', 1, 1)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (4, N'DOKONČENO', 2, 3)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (5, N'ZRUŠENO', 2, 2)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (6, N'NEOCENĚNO', 0, 6)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (7, N'ROZPRACOVÁNO', 0, 1)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (8, N'PROBÍHÁ OCENĚNÍ', 0, 4)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (9, N'DOŽÁDÁNÍ', 0, 5)
GO
INSERT [dbo].[WorkflowTaskStatesNoby] ([Id], [Name], [Filter], [Indicator]) VALUES (10, N'DOPLNĚNÍ DOKUMENTŮ', 0, 1)
GO
