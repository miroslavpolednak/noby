IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[[xxvvss_v33PMP_User_S]]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[xxvvss_v33PMP_User_S]

CREATE SYNONYM [dbo].[xxvvss_v33PMP_User_S] FOR [BABETA].[xxvvss].[dbo].[v33PMP_User]