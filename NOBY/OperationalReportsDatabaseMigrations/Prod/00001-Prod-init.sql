IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NOBY_Case') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Case]

CREATE SYNONYM [dbo].[NOBY_Case] FOR [BABETA.VSSKB.CZ].[CaseService].[dbo].[Case]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_Offer]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Offer]

CREATE SYNONYM [dbo].[NOBY_Offer] FOR [BABETA.VSSKB.CZ].[OfferService].[dbo].[Offer]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_SalesArrangement]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_SalesArrangement]

CREATE SYNONYM [dbo].[NOBY_SalesArrangement] FOR [BABETA.VSSKB.CZ].[SalesArrangementService].[dbo].[SalesArrangement]