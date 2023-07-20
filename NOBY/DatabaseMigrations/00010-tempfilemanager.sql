DROP TABLE IF EXISTS dbo.TempFile
GO

CREATE TABLE [dbo].[TempFile](
	[TempFileId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[MimeType] [varchar](500) NOT NULL,
	[ObjectId] [bigint] NULL,
	[ObjectType] [varchar](50) NULL,
	[SessionId] [uniqueidentifier] NULL,
	[CreatedUserName] [nvarchar](100) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TempFile] PRIMARY KEY CLUSTERED 
(
	[TempFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO