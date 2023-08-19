DROP TRIGGER IF EXISTS AuditEventCreated;
GO
DROP TRIGGER IF EXISTS AuditEventModified
GO
DROP SEQUENCE IF EXISTS dbo.NobyLoggerSequence
GO
DROP TABLE IF EXISTS dbo.AuditEvent;
GO

CREATE TABLE [dbo].[AuditEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [varchar](max) NOT NULL,
	[AuditEventTypeId] [varchar](20) NOT NULL,
	[Detail] [nvarchar](max) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[CreatedBy] [varchar](150) NULL,
	[CreatedTime] [datetime] NULL,
	[ModifiedBy] [varchar](150) NULL,
	[ModifiedTime] [datetime] NULL,
 CONSTRAINT [PK_AuditEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditEvent] ADD  CONSTRAINT [DF_AuditEvent_EventID]  DEFAULT (newid()) FOR [EventID]
GO

ALTER TABLE [dbo].[AuditEvent] ADD  CONSTRAINT [DF_AuditEvent_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO


CREATE TRIGGER AuditEventCreated
ON dbo.AuditEvent
AFTER INSERT AS
UPDATE dbo.AuditEvent SET [CreatedBy]=SYSTEM_USER, [CreatedTime]=GETDATE() FROM inserted WHERE AuditEvent.Id=inserted.Id;
GO


CREATE TRIGGER AuditEventModified
ON dbo.AuditEvent
INSTEAD OF UPDATE AS
UPDATE dbo.AuditEvent SET [ModifiedBy]=SYSTEM_USER, [ModifiedTime]=GETDATE() FROM inserted WHERE AuditEvent.Id=inserted.Id;
GO


CREATE SEQUENCE dbo.NobyLoggerSequence
AS bigint
START WITH 1
INCREMENT BY 1
NO MAXVALUE
NO CYCLE
CACHE 15
GO
