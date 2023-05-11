ALTER TABLE Offer ADD IsCreditWorthinessSimpleRequested bit NOT NULL CONSTRAINT D_Offer_IsCreditWorthinessSimpleRequested DEFAULT (0)
ALTER TABLE Offer ADD CreditWorthinessSimpleInputs NVARCHAR(max)
ALTER TABLE Offer ADD CreditWorthinessSimpleInputsBin varbinary(max)
