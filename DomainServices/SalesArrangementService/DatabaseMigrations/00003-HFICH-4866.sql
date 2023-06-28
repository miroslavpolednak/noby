IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FlowSwitch]') AND type in (N'U'))
ALTER TABLE [dbo].[FlowSwitch] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[FlowSwitch]
GO
DROP TABLE IF EXISTS [dbo].[FlowSwitchHistory]
GO

/****** Object:  Table [dbo].[FlowSwitch]    Script Date: 14.03.2023 21:41:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FlowSwitch](
	[FlowSwitchId] [int] NOT NULL,
	[SalesArrangementId] [int] NOT NULL,
	[Value] [bit] NOT NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_FlowSwitch] PRIMARY KEY CLUSTERED 
(
	[FlowSwitchId] ASC,
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[FlowSwitchHistory])
)
GO
