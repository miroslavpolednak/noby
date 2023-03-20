DROP TABLE IF EXISTS dbo.[MigrationHistory]
GO

DROP TABLE IF EXISTS [dbo].[ServiceDiscovery]
GO

CREATE TABLE [dbo].[ServiceDiscovery](
	[EnvironmentName] [varchar](20) NOT NULL,
	[ServiceName] [varchar](100) NOT NULL,
	[ServiceUrl] [varchar](500) NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
 CONSTRAINT [PK_ServiceDiscovery] PRIMARY KEY CLUSTERED 
(
	[ServiceName] ASC,
	[ServiceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
