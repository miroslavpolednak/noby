ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = OFF  );
GO

ALTER TABLE dbo.ActiveTask ALTER COLUMN TaskProcessId bigint;
GO
ALTER TABLE dbo.ActiveTaskHistory ALTER COLUMN TaskProcessId bigint;
GO

ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ActiveTaskHistory]) );
GO