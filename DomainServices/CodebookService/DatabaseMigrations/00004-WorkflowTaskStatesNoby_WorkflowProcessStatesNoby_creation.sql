-- WorkflowTaskStatesNoby
CREATE TABLE [dbo].[WorkflowTaskStatesNoby](
	[Id] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Filter] [nvarchar](50) NOT NULL,
	[Indicator] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WorkflowTaskStatesNoby] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- WorkflowProcessStatesNoby
CREATE TABLE [dbo].[WorkflowProcessStatesNoby](
	[Id] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Indicator] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WorkflowProcessStatesNoby] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO