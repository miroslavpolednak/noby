DROP TABLE IF EXISTS [DDS].[SmsData]
GO
CREATE  TABLE [DDS].[SmsData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
 CONSTRAINT [PK_SmsData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO

DROP TABLE IF EXISTS [DDS].[EmailData]
GO
CREATE TABLE [DDS].[EmailData](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
 CONSTRAINT [PK_EmailData] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO

DROP TABLE IF EXISTS [dbo].Notification
GO
CREATE TABLE [dbo].[Notification](
	[Id] [uniqueidentifier] NOT NULL,
	[Channel] [int] NOT NULL,
	[State] [int] NOT NULL,
	[Identity] [varchar](100) NULL,
	[IdentityScheme] int NULL,
	[CaseId] bigint NULL,
	[CustomId] [varchar](450) NULL,
	[DocumentId] [varchar](450) NULL,
	[DocumentHash] [varchar](max) NULL,
	[HashAlgorithm] [int] NULL,
	[CreatedUserName] [varchar](100) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ResultTime] [datetime] NULL,
	[Errors] [nvarchar](max) NULL,
	[Mandant] tinyint NULL,
	[Resend] bit NOT NULL
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO


