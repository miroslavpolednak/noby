ALTER TABLE dbo.EmailResult ADD
	[SenderType] tinyint NOT NULL CONSTRAINT DF_EmailResult_SenderType DEFAULT 0,
	[Resend] bit NOT NULL CONSTRAINT DF_EmailResult_Resend DEFAULT 0
GO

CREATE TABLE [dbo].[SentNotification](
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SentNotification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Create SCHEMA DDS
GO
CREATE TABLE [DDS].[SendEmail](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL
 CONSTRAINT [PK_SendEmail] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
