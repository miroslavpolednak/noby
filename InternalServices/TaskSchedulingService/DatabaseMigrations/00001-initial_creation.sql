DROP TABLE IF EXISTS [dbo].[ScheduleLock];
GO
CREATE TABLE [dbo].[ScheduleLock](
	[InstanceName] [varchar](100) NOT NULL,
	[LockedUntil] [datetime] NOT NULL
) ON [PRIMARY]
GO

DROP TABLE IF EXISTS [dbo].[ScheduleTrigger]
GO

CREATE TABLE [dbo].[ScheduleTrigger](
	[ScheduleTriggerId] [uniqueidentifier] NOT NULL,
	[ScheduleJobId] [uniqueidentifier] NOT NULL,
	[TriggerName] [nvarchar](250) NOT NULL,
	[Cron] [varchar](10) NOT NULL,
	[JobData] nvarchar(max) NULL,
	[IsDisabled] [bit] NOT NULL,
 CONSTRAINT [PK_ScheduleTrigger] PRIMARY KEY CLUSTERED 
(
	[ScheduleTriggerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScheduleTrigger] ADD  CONSTRAINT [DF_ScheduleTrigger_IsDisabled]  DEFAULT ((0)) FOR [IsDisabled]
GO

DROP TABLE IF EXISTS [dbo].[ScheduleJob]
GO

CREATE TABLE [dbo].[ScheduleJob](
	[ScheduleJobId] [uniqueidentifier] NOT NULL,
	[JobName] [nvarchar](250) NOT NULL,
	[JobType] [varchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDisabled] [bit] NOT NULL,
 CONSTRAINT [PK_ScheduleJob] PRIMARY KEY CLUSTERED 
(
	[ScheduleJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScheduleJob] ADD  CONSTRAINT [DF_ScheduleJob_IsDisabled]  DEFAULT ((0)) FOR [IsDisabled]
GO

DROP TABLE IF EXISTS [dbo].[ScheduleJobStatus]
GO

CREATE TABLE [dbo].[ScheduleJobStatus](
	[ScheduleJobStatusId] [uniqueidentifier] NOT NULL,
	[ScheduleJobId] [uniqueidentifier] NOT NULL,
	[ScheduleTriggerId] [uniqueidentifier] NULL,
	[Status] [varchar](50) NOT NULL,
	[StartedAt] [datetime] NOT NULL,
	[StatusChangedAt] [datetime] NULL,
	[TraceId] [varchar](50) NULL,
 CONSTRAINT [PK_ScheduleJobStatus] PRIMARY KEY CLUSTERED 
(
	[ScheduleJobStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

DROP PROCEDURE IF EXISTS dbo.fn_AcquireScheduleLock
GO

CREATE PROCEDURE dbo.fn_AcquireScheduleLock
	@instanceName varchar(100),
	@lockTimeout int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @lock_instance varchar(100);
    DECLARE @t_instance varchar(100), @t_lock datetime;
	SELECT TOP 1 @t_instance = InstanceName, @t_lock = LockedUntil FROM dbo.ScheduleLock;

	-- jeste neexistuje zadny lock
	IF @t_instance IS NULL
	BEGIN
		INSERT INTO dbo.ScheduleLock (InstanceName, LockedUntil) VALUES (@instanceName, DATEADD(SECOND, @lockTimeout, GETDATE()));
		SET @lock_instance = @instanceName;
	END
	-- zamek existuje a je na aktualni instanci
	ELSE IF @t_instance = @instanceName OR @t_lock < GETDATE()
	BEGIN
		UPDATE dbo.ScheduleLock SET InstanceName = @instanceName, LockedUntil = DATEADD(SECOND, @lockTimeout, GETDATE());
		SET @lock_instance = @instanceName;
	END
	-- zamek drzi jina instance
	ELSE
	BEGIN
		SET @lock_instance = @t_instance;
	END

	SELECT CAST(CASE WHEN @lock_instance = @instanceName THEN 1 ELSE 0 END as bit) 'IsLockAcquired', @lock_instance 'LockOwnerInstanceName';
END
GO