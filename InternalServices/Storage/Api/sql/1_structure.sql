SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Blob](
	[BlobKey] [uniqueidentifier] NOT NULL,
	[ApplicationKey] [varchar](50) NOT NULL,
	[SessionId] [varchar](50) NULL,
	[BlobName] [nvarchar](500) NULL,
	[BlobLength] [bigint] NOT NULL,
	[BlobContentType] [varchar](500) NULL,
	[Kind] [tinyint] NULL,
	[InsertTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Blob] PRIMARY KEY CLUSTERED 
(
	[BlobKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO