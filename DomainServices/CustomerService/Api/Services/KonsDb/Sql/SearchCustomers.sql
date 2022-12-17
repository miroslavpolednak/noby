SELECT Id PartnerId,
	Jmeno FirstName,
	Prijmeni LastName,
	Pohlavi GenderId,
	RodneCisloIco BirthNumber,
	DatumNarozeni BirthDate,
	Ulice Street,
	Misto City,
	Psc PostCode,
	PrukazTotoznosti IdentificationDocumentNumber,
	TypDokladu IdentificationDocumentTypeId,
	PrukazVydalDatum IdentificationDocumentIssuedOn,
	PrukazVydal IdentificationDocumentIssuedBy,
	PrukazStatVydaniId IdentificationDocumentIssuingCountryId,
	PreukazPlatnostDo IdentificationDocumentValidTo,
	KBId KbId
FROM dbo.PARTNER
WHERE (@SearchByIds = 0 OR Id IN @partnerIds)
    AND (@firstName IS NULL OR Jmeno = @firstName)
	AND (@lastName IS NULL OR Prijmeni = @lastName)
    AND (@birthNumber IS NULL OR RodneCisloIco = @birthNumber)
    AND (@dateOfBirth IS NULL OR DatumNarozeni = @dateOfBirth)
    AND (@documentNumber IS NULL OR (PrukazTotoznosti = @documentNumber AND TypDokladu = @documentTypeId AND PrukazStatVydaniId = @documentIssuingCountryId))
