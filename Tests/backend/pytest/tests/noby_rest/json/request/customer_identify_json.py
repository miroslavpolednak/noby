json_req_customer_identify_full = \
    {
        "firstName": "Marek",
        "lastName": "Mikel",
        "dateOfBirth": "1967-08-30T00:00:00",
        "issuingCountryId": 16,
        "identificationDocumentTypeId": 1,
        "identificationDocumentNumber": "101929795",
        "birthNumber": None,
        "identity": {
            "id": 113666423,
            "scheme": 2
        }
    }

json_req_customer_identify_without_id = \
    {
        "identity":
            {"scheme": 2},
        "firstName": "ANNA",
        "lastName": "NOVÁKOVÁ",
        "dateOfBirth": "1957-08-04T00:00:00.000Z",
        "birthNumber": None,
        "issuingCountryId": 16,
        "identificationDocumentTypeId": 1,
        "identificationDocumentNumber": "LK123462"
    }
