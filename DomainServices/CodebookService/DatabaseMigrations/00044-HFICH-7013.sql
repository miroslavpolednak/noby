UPDATE SqlQuery SET SqlQueryText='SELECT CAST(KOD as int) ''Id'', POPIS ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', DEF ''IsDefault'', CAST(PORADI as int) ''Order'', CAST(ZAJISTUJICI_INSTRUMENT_NOBY as bit) ''Collateral'' FROM [SBR].[HTEDM_CIS_TYPY_NEH_UV]  ORDER BY PORADI ASC' WHERE SqlQueryId='RealEstateTypes';