SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OfferInstance](
	[OfferInstanceId] [int] IDENTITY(1,1) NOT NULL,
	[ProductInstanceType] [int] NULL,
	[ResourceProcessId] [uniqueidentifier] NOT NULL,
	[Inputs] [nvarchar](max) NOT NULL,
	[Outputs] [nvarchar](max) NOT NULL,
	[CreatedUserName] [nvarchar](100) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_OfferModelation] PRIMARY KEY CLUSTERED 
(
	[OfferInstanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ScheduleItems](
	[ScheduleItemsId] [int] IDENTITY(1,1) NOT NULL,
	[OfferInstanceId] [int] NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ScheduleItems] PRIMARY KEY CLUSTERED 
(
	[ScheduleItemsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
