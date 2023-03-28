IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormInstanceInterface]') AND type in (N'U'))
DROP TABLE [dbo].[FormInstanceInterface]
GO