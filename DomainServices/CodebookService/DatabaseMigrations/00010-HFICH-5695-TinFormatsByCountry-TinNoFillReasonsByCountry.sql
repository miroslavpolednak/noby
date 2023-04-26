-- TinFormatsByCountry
CREATE TABLE [dbo].[TinFormatsByCountry](
	[Id] [int] NOT NULL,
	[CountryCode] [nvarchar](10) NOT NULL,
	[RegularExpression] [nvarchar](250) NOT NULL,
	[IsForFo] [bit] NOT NULL,
	[TooltipTooltip] [nvarchar](250) NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_TinFormatsByCountry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- TinNoFillReasonsByCountry
CREATE TABLE [dbo].[TinNoFillReasonsByCountry](
	[Id] [nvarchar](10) NOT NULL,
	[IsTinMandatory] [bit] NOT NULL,
	[ReasonForBlankTin] [nvarchar](250) NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_TinNoFillReasonsByCountry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-- TinFormatsByCountry [data]
INSERT INTO [dbo].[TinFormatsByCountry] ([Id],[CountryCode],[RegularExpression],[IsForFo],[TooltipTooltip],[ValidFrom],[ValidTo]) VALUES
(1, 'Country', 'RegularExpression',1,'TooltipTooltip', '2022-09-20 12:22:36', NULL);
GO

-- TinNoFillReasonsByCountry [data]
INSERT INTO [dbo].[TinNoFillReasonsByCountry] ([Id],[IsTinMandatory],[ReasonForBlankTin],[ValidFrom],[ValidTo]) VALUES
('Code', 1, 'ReasonForBlankTin', '2022-09-20 12:22:36', NULL);
GO