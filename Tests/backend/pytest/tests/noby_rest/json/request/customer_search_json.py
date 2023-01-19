json_req_customer_search_lastname = \
    {
        "searchData": {
            "identity": {
                "id": None,
                "scheme": 2
            },
            "identityId": None,
            "firstName": "",
            "lastName": "mikel",
            "dateOfBirth": None,
            "birthNumber": None,
            "nameJuridicalPerson": None,
            "cin": None,
            "email": None,
            "phone": None,
            "issuingCountryId": None,
            "identificationDocumentTypeId": None,
            "identificationDocumentNumber": None
        },
        "pagination": {
            "recordOffset": 0,
            "pageSize": 10,
            "sorting": [
                {
                    "field": "lastName",
                    "descending": True
                }
            ]
        }
    }

json_req_customer_search_date_of_birth = \
    {
        "searchData": {
            "identity": {
                "id": None, "scheme": 2
            },
            "identityId": None,
            "firstName": "",
            "lastName": "",
            "dateOfBirth": "1989-11-07T00:00:00.000Z",
            "birthNumber": "89110701111",
            "nameJuridicalPerson": None,
            "cin": None,
            "email": None,
            "phone": None,
            "identificationDocumentNumber": None
        },
        "pagination": {
            "recordOffset": 0,
            "pageSize": 10,
            "sorting": [
                {
                    "field": "lastName", "descending": True
                }
            ]
        }
    }
