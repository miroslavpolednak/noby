DELETE FROM EasFormDataField WHERE DataFieldId IN (78, 79, 80, 81, 82, 83)

INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].kod_poplatku', 3, 2, 'OfferFees[].FeeId')
INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].suma_poplatku_sazebnik', 3, 2, 'OfferFees[].TariffSum')
INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].suma_poplatku_skladacka', 3, 2, 'OfferFees[].ComposedSum')
INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].suma_poplatku_vysledna', 3, 2, 'OfferFees[].FinalSum')
INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].slevaIC', 3, 2, 'OfferFees[].DiscountPercentage')
INSERT INTO EasFormSpecialDataField VALUES (2, 'seznam_poplatku[].kodMaAkce', 3, 2, 'OfferFees[].MarketingActionId')