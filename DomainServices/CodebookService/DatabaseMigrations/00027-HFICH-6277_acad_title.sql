UPDATE dbo.SqlQuery SET SqlQueryText='SELECT CAST(KOD as int) ''Id'', [TEXT] ''Name'', CAST(1 as bit) ''IsValid'' from [xxdvss_hf].[SBR].HTEDM_CIS_TITULY WHERE JE_CM=1' WHERE SqlQueryId='AcademicDegreesBefore';

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcademicDegreesBefore]') AND type in (N'U'))
DROP TABLE [dbo].[AcademicDegreesBefore]
GO
