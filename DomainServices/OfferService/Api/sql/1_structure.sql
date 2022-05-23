SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Offer](
	[OfferId] [int] IDENTITY(1,1) NOT NULL,
	[ResourceProcessId] [uniqueidentifier] NOT NULL,
	[BasicParameters] [nvarchar](max) NOT NULL,
	[SimulationInputs] [nvarchar](max) NOT NULL,
	[SimulationResults] [nvarchar](max) NOT NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED 
(
	[OfferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
