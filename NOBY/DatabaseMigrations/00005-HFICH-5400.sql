DROP TABLE [dbo].[FlowSwitch];
GO

CREATE TABLE [dbo].[FlowSwitch](
	[FlowSwitchId] [int] NOT NULL,
	[Name] varchar(50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DefaultValue] [bit] NOT NULL,
 CONSTRAINT [PK_FlowSwitch] PRIMARY KEY CLUSTERED 
(
	[FlowSwitchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

TRUNCATE TABLE [dbo].[FlowSwitch2Group];
GO

INSERT [dbo].[FlowSwitch] ([FlowSwitchId], [Name], [Description], [DefaultValue]) VALUES 
	(1, 'IsOfferGuaranteed', N'Nabídka s platnou garancí', 0),
	(2, 'CustomerIdentifiedOnMainHousehold', N'Identifikovaní klienti na hlavní domácnosti', 0),
	(3, 'CustomerIdentifiedOnCodebtorHousehold', N'Identifikovaní klienti na spolužadatelské domácnosti', 0),
	(4, 'Was3601MainChangedAfterSigning', N'3601 pro hlavní domácnost byla změněna po podpisu', 1),
	(5, 'Was3602CodebtorChangedAfterSigning', N'3602 pro spoludlužnickou domácnost byla změněna po podpisu', 1),
	(6, 'ParametersSavedAtLeastOnce', N'Došlo k uložení parametrů na žádosti', 0);
GO

INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES 
	(1, 1, 3, 1),
	(2, 1, 3, 1),
	(5, 1, 3, 1),
	(3, 2, 3, 1),
	(5, 2, 3, 1),
	(3, 3, 3, 1),
	(5, 3, 3, 1),
	(5, 4, 3, 0),
	(6, 4, 3, 0),
	(5, 5, 3, 0),
	(6, 5, 3, 0),
	(4, 6, 3, 1),
	(5, 6, 3, 1);
