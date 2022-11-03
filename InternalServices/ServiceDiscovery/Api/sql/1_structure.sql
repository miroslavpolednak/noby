SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceDiscovery](
	[EnvironmentName] [varchar](50) NULL,
	[ServiceName] [varchar](50) NULL,
	[ServiceUrl] [varchar](250) NULL,
	[ServiceType] [tinyint] NULL
) ON [PRIMARY]
GO