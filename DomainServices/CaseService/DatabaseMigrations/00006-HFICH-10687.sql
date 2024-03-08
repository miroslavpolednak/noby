CREATE TABLE [dbo].[ConfirmedPriceException](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[TaskIdSB] [int] NOT NULL,
	[ConfirmedDate] [date] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ConfirmedPriceException] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
