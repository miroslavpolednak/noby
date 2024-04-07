drop table if exists [dbo].[ResponseCode];
GO
CREATE TABLE [dbo].[ResponseCode](
	[ResponseCodeId] [int] IDENTITY(1,1) NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[ResponseCodeTypeId] [int] NOT NULL,
	[Data] [nvarchar](500) NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ResponseCode] PRIMARY KEY CLUSTERED 
(
	[ResponseCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

drop index if exists [idxCaseId] ON [dbo].[ResponseCode]
go
CREATE NONCLUSTERED INDEX [idxCaseId] ON [dbo].[ResponseCode]
(
	[CaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
