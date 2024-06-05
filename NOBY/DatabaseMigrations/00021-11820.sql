DROP TABLE IF EXISTS dbo.FeBanner;
GO

CREATE TABLE [dbo].[FeBanner](
	[FeBannerId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Severity] [int] NOT NULL,
	[VisibleFrom] [datetime] NOT NULL,
	[VisibleTo] [datetime] NOT NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_FeBanner] PRIMARY KEY CLUSTERED 
(
	[FeBannerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FeBanner] ADD  CONSTRAINT [DF_FeBanner_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]
GO
