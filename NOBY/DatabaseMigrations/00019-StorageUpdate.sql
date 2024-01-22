DROP TABLE IF EXISTS [dbo].[TempStorageItem];

CREATE TABLE [dbo].[TempStorageItem](
	[TempStorageItemId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[MimeType] [varchar](500) NOT NULL,
	[ObjectId] [bigint] NULL,
	[ObjectType] [varchar](50) NULL,
	[SessionId] [uniqueidentifier] NULL,
	[TraceId] [varchar](70) NULL,
 CONSTRAINT [PK_TempStorageItem] PRIMARY KEY CLUSTERED 
(
	[TempStorageItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO