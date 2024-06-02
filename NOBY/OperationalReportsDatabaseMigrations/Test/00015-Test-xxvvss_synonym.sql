IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xxvvss_GetUserIdentities_S]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[xxvvss_GetUserIdentities_S]

CREATE SYNONYM [dbo].[xxvvss_GetUserIdentities_S] FOR [ADPRA175].[xxvvss].[dbo].[getUserIdentities]