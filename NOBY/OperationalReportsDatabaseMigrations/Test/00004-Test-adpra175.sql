IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NOBY_Case') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Case]

CREATE SYNONYM [dbo].[NOBY_Case] FOR [ADPRA175].[CaseService].[dbo].[Case]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_Offer]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Offer]

CREATE SYNONYM [dbo].[NOBY_Offer] FOR [ADPRA175].[OfferService].[dbo].[Offer]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_OfferData]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_OfferData]

CREATE SYNONYM [dbo].[NOBY_OfferData] FOR [ADPRA175].[OfferService].[DDS].[OfferData]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_SalesArrangement]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_SalesArrangement]

CREATE SYNONYM [dbo].[NOBY_SalesArrangement] FOR [ADPRA175].[SalesArrangementService].[dbo].[SalesArrangement]