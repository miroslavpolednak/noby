insert into [dbo].[SqlQuery] (SqlQueryId, SqlQueryText, DatabaseProvider) values ('FeeChangeRequests','SELECT KOD ''Id'', TEXT ''ShortName'', POPLATOK ''Amount'', CAST(CASE WHEN CAST(GETDATE() as date) BETWEEN ISNULL(DATUM_PLATNOSTI_OD, ''1901-01-01'') AND ISNULL(DATUM_PLATNOSTI_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].HTEDM_CIS_POPLATOK_ZM_NAVRH',2);
