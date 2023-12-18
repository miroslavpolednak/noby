UPDATE EasFormSpecialDataField 
SET FieldPath = 'HouseholdData.Customers[].Obligations[].CreditorName'
WHERE JsonPropertyName = 'seznam_ucastniku[].klient.seznam_zavazku[].veritel_nazev'