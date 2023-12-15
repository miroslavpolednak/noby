EXEC sp_rename '[NotificationService].[dbo].[EmailResult].[Resent]', 'Resend', 'COLUMN';
GO
ALTER TABLE [NotificationService].[dbo].[EmailResult] DROP CONSTRAINT [DF_EmailResult_Resent];
GO
ALTER TABLE [NotificationService].[dbo].[EmailResult]
    ADD CONSTRAINT [DF_EmailResult_Resend] DEFAULT ((0)) FOR [Resend];