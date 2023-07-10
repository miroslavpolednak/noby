ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = OFF  )
GO

ALTER TABLE [dbo].[ActiveTask] ADD TaskIdSb INT NOT NULL DEFAULT(0);
GO
ALTER TABLE [dbo].[ActiveTaskHistory] ADD TaskIdSb INT NOT NULL DEFAULT(0);
GO
sp_rename 'ActiveTask.TaskProcessId', 'TaskId', 'COLUMN';
GO
sp_rename 'ActiveTaskHistory.TaskProcessId', 'TaskId', 'COLUMN';
GO

UPDATE [dbo].[ActiveTask] SET TaskIdSb=0;
UPDATE [dbo].[ActiveTaskHistory] SET TaskIdSb=0;

ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ActiveTaskHistory])  )
GO
