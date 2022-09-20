SELECT Id PartnerId,
	Jmeno FirstName,
	Prijmeni LastName,
	Pohlavi GenderId,
	RodneCisloIco BirthNumber,
	DatumNarozeni BirthDate,
	PrukazTotoznosti IdentificationDocumentNumber,
	TypDokladu IdentificationDocumentTypeId,
	PrukazVydalDatum IdentificationDocumentIssuedOn,
	PrukazVydal IdentificationDocumentIssuedBy,
	PrukazStatVydaniId IdentificationDocumentIssuingCountryId,
	PreukazPlatnostDo IdentificationDocumentValidTo
FROM dbo.PARTNER
WHERE Id IN @partnerIds
