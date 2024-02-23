IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_OfferData]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_OfferData]

CREATE SYNONYM [dbo].[NOBY_OfferData] FOR [BABETA].[OfferService].[DDS].[OfferData]