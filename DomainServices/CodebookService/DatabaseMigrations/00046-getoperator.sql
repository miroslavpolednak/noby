UPDATE SqlQuery SET SqlQueryText='SELECT CAST(KOD_OPERATORA as int) ''PerformerCode'', JMENO ''PerformerName'', [LOGIN_MP] ''PerformerLogin'' FROM [SBR].HTEDM_OP_AD WHERE [LOGIN_MP]=@PerformerLogin', DatabaseProvider=2 WHERE SqlQueryId='GetOperator';