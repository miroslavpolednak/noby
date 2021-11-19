CREATE TABLE [Blob](
	[BlobKey] varchar(50) PRIMARY KEY,
	[ApplicationKey] [varchar](50) NOT NULL,
	[SessionId] [varchar](50) NULL,
	[BlobName] [nvarchar](500) NULL,
	[BlobLength] [bigint] NOT NULL,
	[BlobContentType] [varchar](500) NULL,
	[Kind] [tinyint] NOT NULL,
	[InsertTime] [datetime] NOT NULL
);
