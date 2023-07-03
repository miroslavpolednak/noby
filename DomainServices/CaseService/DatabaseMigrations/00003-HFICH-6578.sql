ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = OFF  )
GO

ALTER TABLE [dbo].[ActiveTask] ADD TaskIdSb INT NOT NULL;
GO
ALTER TABLE [dbo].[ActiveTaskHistory] ADD TaskIdSb INT NOT NULL;
GO
UPDATE [dbo].[ActiveTask] SET TaskIdSb=0;
UPDATE [dbo].[ActiveTaskHistory] SET TaskIdSb=0;

ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ActiveTaskHistory])  )
GO
