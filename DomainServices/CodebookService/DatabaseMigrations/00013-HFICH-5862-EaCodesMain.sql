-- EaCodesMainExtension
CREATE TABLE [dbo].[EaCodesMainExtension](
	[EaCodesMainId] [int] NOT NULL,
	[IsFormIdRequested] [bit] NOT NULL,
 CONSTRAINT [PK_EaCodesMainExtension] PRIMARY KEY CLUSTERED 
(
	[EaCodesMainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-- EaCodesMainExtension [data]
INSERT INTO [dbo].[EaCodesMainExtension] ([EaCodesMainId],[IsFormIdRequested]) VALUES
(605569, 1),
(608248, 1),
(608578, 1),
(613226, 1),
(608279, 1),
(608580, 1),
(608524, 1),
(616578, 1),
(616525, 1),
(608522, 1),
(608243, 1);
GO