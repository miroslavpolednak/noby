INSERT INTO [dbo].[SqlQuery] ([SqlQueryId],[SqlQueryText],[DatabaseProvider]) VALUES ('ExtraPaymentReasons','SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN CAST(GETDATE() as date) BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', PORADI ''Order'' FROM [SBR].HTEDM_CIS_PREDCASNE_SPLATENIE_DOVOD ORDER BY PORADI ASC',2);
