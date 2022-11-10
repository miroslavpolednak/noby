import requests


def test_get_customer(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/customer/get?identityScheme=2&identityId=113666423",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    natural_person = response['naturalPerson']
    updatable = response['updatable']
    islegally_incapable = response['isLegallyIncapable']
    addresses = response['addresses']
    contacts = response['contacts']
    identification_document = response['identificationDocument']

    assert natural_person == {'birthNumber': '6708301490', 'firstName': 'KLIENT_MAREK', 'lastName': 'TEST_MIKEL', 'dateOfBirth': '1967-08-30T00:00:00', 'degreeBeforeId': 47, 'birthName': 'Alešovka', 'placeOfBirth': 'Praha 4', 'birthCountryId': 16, 'gender': 0, 'maritalStatusStateId': 2, 'citizenshipCountriesId': [16]}
    assert updatable == False
    assert islegally_incapable == False
    assert addresses == [{'addressTypeId': 1, 'isPrimary': True, 'street': 'Bryksova', 'buildingIdentificationNumber': '550', 'landRegistryNumber': '30', 'postcode': '783 01', 'city': 'Olomouc - Slavonín'}]
    assert contacts == [{'isPrimary': True, 'contactTypeId': 1, 'value': '+420 777888999'}, {'isPrimary': True, 'contactTypeId': 5, 'value': 'kuk@kuk.cz'}]
    assert identification_document == {'identificationDocumentTypeId': 1, 'validTo': '2029-05-16T00:00:00', 'issuedBy': 'MěÚ Kotěhůlky', 'issuingCountryId': 16, 'issuedOn': '2022-05-16T00:00:00', 'registerPlace': ''}