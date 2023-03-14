/****** Object:  Table [dbo].[FlowSwitchGroup]    Script Date: 14.03.2023 21:41:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FlowSwitchGroup]') AND type in (N'U'))
DROP TABLE [dbo].[FlowSwitchGroup]
GO
/****** Object:  Table [dbo].[FlowSwitch2Group]    Script Date: 14.03.2023 21:41:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FlowSwitch2Group]') AND type in (N'U'))
DROP TABLE [dbo].[FlowSwitch2Group]
GO
/****** Object:  Table [dbo].[FlowSwitch]    Script Date: 14.03.2023 21:41:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FlowSwitch]') AND type in (N'U'))
DROP TABLE [dbo].[FlowSwitch]
GO
/****** Object:  Table [dbo].[FlowSwitch]    Script Date: 14.03.2023 21:41:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowSwitch](
	[FlowSwitchId] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DefaultValue] [bit] NOT NULL,
 CONSTRAINT [PK_FlowSwitch] PRIMARY KEY CLUSTERED 
(
	[FlowSwitchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FlowSwitch2Group]    Script Date: 14.03.2023 21:41:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowSwitch2Group](
	[FlowSwitchGroupId] [int] NOT NULL,
	[FlowSwitchId] [int] NOT NULL,
	[GroupType] [int] NOT NULL,
	[Value] [bit] NOT NULL,
 CONSTRAINT [PK_FlowSwitch2Group] PRIMARY KEY CLUSTERED 
(
	[FlowSwitchGroupId] ASC,
	[FlowSwitchId] ASC,
	[GroupType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FlowSwitchGroup]    Script Date: 14.03.2023 21:41:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlowSwitchGroup](
	[FlowSwitchGroupId] [int] NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsVisibleDefault] [bit] NOT NULL,
	[IsActiveDefault] [bit] NOT NULL,
	[IsCompletedDefault] [bit] NOT NULL,
 CONSTRAINT [PK_FlowSwitchGroup] PRIMARY KEY CLUSTERED 
(
	[FlowSwitchGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[FlowSwitch] ([FlowSwitchId], [Description], [DefaultValue]) VALUES (1, N'Nabídka s platnou garancí', 0)
GO
INSERT [dbo].[FlowSwitch] ([FlowSwitchId], [Description], [DefaultValue]) VALUES (2, N'Požadavek na IC', 0)
GO
INSERT [dbo].[FlowSwitch] ([FlowSwitchId], [Description], [DefaultValue]) VALUES (3, N'Alespoň jeden příjem na hlavní domácnosti', 0)
GO
INSERT [dbo].[FlowSwitch] ([FlowSwitchId], [Description], [DefaultValue]) VALUES (4, N'Alespoň jeden příjem na spoludlužnícké domácnosti', 0)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (1, 1, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (2, 1, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (2, 2, 1, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (2, 2, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (3, 3, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (3, 4, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (5, 3, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (5, 4, 3, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (6, 3, 2, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (6, 4, 2, 1)
GO
INSERT [dbo].[FlowSwitch2Group] ([FlowSwitchGroupId], [FlowSwitchId], [GroupType], [Value]) VALUES (8, 1, 2, 1)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (1, N'Modelation Section
', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (2, N'Individual Price Section', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (3, N'Household Section
', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (4, N'Parameters Section
', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (5, N'Signing Section', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (6, N'Scoring Section', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (7, N'Evaluation Section', 1, 1, 0)
GO
INSERT [dbo].[FlowSwitchGroup] ([FlowSwitchGroupId], [Description], [IsVisibleDefault], [IsActiveDefault], [IsCompletedDefault]) VALUES (8, N'Send Button', 1, 1, 1)
GO
