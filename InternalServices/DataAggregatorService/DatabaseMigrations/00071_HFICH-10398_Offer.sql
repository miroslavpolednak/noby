UPDATE DataField SET FieldPath = 'Offer.Data.OfferId' WHERE DataFieldId = 28
UPDATE DataField SET FieldPath = REPLACE(FieldPath, 'Offer.', 'Offer.MortgageOffer.') WHERE DataServiceId = 3 AND DataFieldId <> 28