import requests
import pytest
from Tests.backend.pytest.tests.noby_rest.db_util import sort_response_id, sort_response_productTypeId, sort_response_paymentDay


def test_codebook_caseStates(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=CaseStates",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    code = response[0]['code']
    codebook = response[0]['codebook']
    assert len(codebook) == 7
    assert code == 'CaseStates'
    assert sort_response_id(codebook) == [
        {'id': 0, 'name': 'unknown'},
        {"id": 1, "name": "Příprava žádosti"},
        {"id": 2, "name": "Zpracování žádosti v továrně"},
        {"id": 3, "name": "Podepisování"},
        {"id": 4, "name": "Čerpání"},
        {"id": 5, "name": "Správa"},
        {"id": 6, "name": "Ukončeno"}
    ]


@pytest.mark.skip('EmployentTypes is not implemented')
def test_codebook_employmentTypes(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=EmployentTypes",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)


# TODO: nesedi response s real
def test_codebook_fees(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=Fees",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_id(codebook)
    FEE001 = sorted_codebook[0]
    FEE008 = sorted_codebook[2]
    assert len(codebook) == 34
    assert FEE001 == {'id': 2001, 'idKb': 'FEE001', 'MandantId': 2, 'shortName': 'Zpracování žádosti', 'name': 'Zpracování a vyhodnocení žádosti o úvěr'}
    assert FEE008 == {'id': 2008, 'idKb': 'FEE008', 'MandantId': 2, 'shortName': 'Poplatek za výpis', 'name': 'Výpis z úvěrového účtu (měsíčně)'}


# TODO: nesedi pocet radku v response 152 vs 127
def test_codebook_fixedRatePeriods(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=FixedRatePeriods",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_productTypeId(codebook)
    Type20001_60_2 = sorted_codebook[32]
    Type20001_72_3 = sorted_codebook[45]
    Type20010_36_1 = sorted_codebook[97]
    Type20010_24_4 = sorted_codebook[124]
    assert len(codebook) == 127
    assert Type20001_60_2 == {'productTypeId': 20001, 'fixedRatePeriod': 60, 'mandantId': 2, 'isNewProduct': True, 'interestRateAlgorithm': 2}
    assert Type20001_72_3 == {'productTypeId': 20001, 'fixedRatePeriod': 72, 'mandantId': 2, 'isNewProduct': True, 'interestRateAlgorithm': 3}
    assert Type20010_36_1 == {'productTypeId': 20010, 'fixedRatePeriod': 36, 'mandantId': 2, 'isNewProduct': True, 'interestRateAlgorithm': 1}
    assert Type20010_24_4 == {'productTypeId': 20010, 'fixedRatePeriod': 24, 'mandantId': 2, 'isNewProduct': True, 'interestRateAlgorithm': 4}


@pytest.mark.skip('Not implemented')
def test_codebook_channels(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=Channels",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_productTypeId(codebook)
    assert len(codebook) == 6
    assert sort_response_id(codebook) == [
        {'Id': 1, 'MandantId': 1, 'Name': 'Interní síť MP', 'IsValid': True},
        {'Id': 2, 'MandantId': 1, 'Name': 'Hypocentrum MP', 'IsValid': True},
        {'Id': 3, 'MandantId': 1, 'Name': 'Agentury MP', 'IsValid': True},
        {'Id': 4, 'MandantId': 2, 'Name': 'Interní síť KB', 'Code': 'BR', 'IsValid': True},
        {'Id': 6, 'MandantId': 2, 'Name': 'Agentury KB', 'Code': 'MC', 'IsValid': True},
        {'Id': 10, 'MandantId': 1, 'Name': 'POS MP', 'IsValid': True}]


@pytest.mark.skip('Not implemented')
def test_codebook_identitySchemes(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=identitySchemes",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    code = response[0]['code']
    codebook = response[0]['codebook']
    print(code)
    print(codebook)
    assert code == 'IdentitySchemes'
    assert len(codebook) == 0
    assert sort_response_id(codebook) == []


def test_codebook_loanKinds(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=LoanKinds",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_id(codebook)
    standard = sorted_codebook[13]
    hypo_bez_nem = sorted_codebook[14]
    not_show_notvalid_id = sorted_codebook[2]['id']
    assert standard == {
        'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True}
    assert hypo_bez_nem == {
        'id': 2001, 'mandantId': 2, 'name': 'Hypotéka bez nemovitosti', 'isDefault': False}
    assert len(codebook) == 15
    assert not_show_notvalid_id != 2
    assert not_show_notvalid_id == 3


#TODO: out od range - nesedi response s real
def test_codebook_loanPurposes(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=LoanPurposes",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_id(codebook)
    koupe_2 = sorted_codebook[62]
    koupe_1 = sorted_codebook[63]
    rekonstrukce_2 = sorted_codebook[66]
    rekonstrukce_1 = sorted_codebook[67]
    koupe_Invalid = sorted_codebook[1]

    assert len(codebook) == 79
    assert koupe_2 == {'id': 202, 'name': 'koupě', 'mandantId': 2, 'productTypeIds': [20001, 20002, 20003], 'order': 2}
    assert koupe_1 == {'id': 202, 'name': 'koupě', 'mandantId': 1, 'order': 2}
    assert rekonstrukce_2 == {'id': 204, 'name': 'rekonstrukce', 'mandantId': 2, 'productTypeIds': [20001, 20002, 20005], 'order': 4}
    assert rekonstrukce_1 == {'id': 204, 'name': 'rekonstrukce', 'mandantId': 1, 'order': 4}
    assert koupe_Invalid == {'id': 2, 'name': 'koupě nového bytu', 'mandantId': 1, 'order': 12}


def test_codebook_maritalStatuses(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=MaritalStatuses",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    code = response[0]['code']
    codebook = response[0]['codebook']
    assert len(codebook) == 7
    assert code == 'MaritalStatuses'
    assert sort_response_id(codebook) == [
        {'id': 0, 'isDefault': True, 'name': 'neuveden'},
        {'id': 1, 'isDefault': False, 'name': 'svobodný/á', 'rdmMaritalStatusCode': 'S'},
        {'id': 2, 'isDefault': False, 'name': 'ženatý/vdaná', 'rdmMaritalStatusCode': 'M'},
         {'id': 3, 'isDefault': False, 'name': 'rozvedený/á', 'rdmMaritalStatusCode': 'D'},
         {'id': 4, 'isDefault': False, 'name': 'vdovec/vdova', 'rdmMaritalStatusCode': 'W'},
         {'id': 5, 'isDefault': False, 'name': 'druh/družka'},
         {'id': 6, 'isDefault': False, 'name': 'registrovaný/á partner/ka', 'rdmMaritalStatusCode': 'R'}
    ]


@pytest.mark.skip('Not implemented')
def test_codebook_marketingActions(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=MarketingActions",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    sorted_codebook = sort_response_productTypeId(codebook)
    assert len(codebook) == 6
    assert sort_response_id(codebook) == [
        {'Id': 1, 'MandantId': 1, 'Name': 'Interní síť MP', 'IsValid': True},
        {'Id': 2, 'MandantId': 1, 'Name': 'Hypocentrum MP', 'IsValid': True},
        {'Id': 3, 'MandantId': 1, 'Name': 'Agentury MP', 'IsValid': True},
        {'Id': 4, 'MandantId': 2, 'Name': 'Interní síť KB', 'Code': 'BR', 'IsValid': True},
        {'Id': 6, 'MandantId': 2, 'Name': 'Agentury KB', 'Code': 'MC', 'IsValid': True},
        {'Id': 10, 'MandantId': 1, 'Name': 'POS MP', 'IsValid': True}]


#TODO: response neni mandantId a nesedi response s real
def test_codebook_paymentDays(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=paymentDays",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    assert len(codebook) == 7
    assert sort_response_paymentDay(codebook) == [
        {'PaymentDay': 10, 'PaymentAccountDay': 10, 'MandantId': 2},
        {'PaymentDay': 10, 'PaymentAccountDay': 31, 'MandantId': 1},
        {'PaymentDay': 15, 'PaymentAccountDay': 15, 'MandantId': 2},
        {'PaymentDay': 15, 'PaymentAccountDay': 31, 'MandantId': 1},
        {'PaymentDay': 20, 'PaymentAccountDay': 20, 'MandantId': 2},
        {'PaymentDay': 20, 'PaymentAccountDay': 31, 'MandantId': 1},
        {'PaymentDay': 25, 'PaymentAccountDay': 31, 'MandantId': 1, 'IsDefault': True}
    ]


# TODO: response vs. real chybne. chybi konsdbloantype a mphomeapiloantype - neni ale specifikovano
def test_codebook_productTypes(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=ProductTypes",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    code = response[0]['code']
    codebook = response[0]['codebook']
    print(code)
    print(codebook)
    assert code == 'ProductTypes'
    assert len(codebook) == 5
    assert codebook == [
        {'id': 20001, 'name': 'Hypoteční úvěr', 'mandantId': 2, 'loanAmountMin': 200000, 'loanAmountMax': 999999999, 'loanDurationMin': 5, 'loanDurationMax': 30, 'ltvMin': 0, 'ltvMax': 90, 'MpHomeApiLoanType': 'KBMortgage',
         'loanKinds': [
             {'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True},
             {'id': 2001, 'mandantId': 2, 'name': 'Hypotéka bez nemovitosti', 'isDefault': False},
         ],
         'KonsDbLoanType': 3},
        {'id': 20004, 'name': 'Hypoteční úvěr - neúčelová část', 'mandantId': 2, 'loanAmountMin': 50000, 'loanAmountMax': 800000, 'loanDurationMin': 5, 'loanDurationMax': 30, 'ltvMin': 0, 'ltvMax': 70, 'MpHomeApiLoanType': 'KBMortgageLoanNonPurposePart',
         'loanKinds': [
             {'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True}
         ],
         'KonsDbLoanType': 6},
        {'id': 20010, 'name': 'Americká hypotéka', 'mandantId': 2, 'loanAmountMin': 200000, 'loanAmountMax': 10000000, 'loanDurationMin': 1, 'loanDurationMax': 20, 'ltvMin': 0, 'ltvMax': 70, 'MpHomeApiLoanType': 'KBAmericanMortgage',
         'loanKinds': [
             {'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True}
         ],
         'KonsDbLoanType': 7},
        {'id': 20002, 'name': 'Hypoteční překlenovací úvěr', 'mandantId': 2, 'loanAmountMin': 200000, 'loanAmountMax': 10000000, 'loanDurationMin': 5, 'loanDurationMax': 30, 'ltvMin': 0, 'ltvMax': 70, 'MpHomeApiLoanType': 'KBBridgingMortgageLoan',
         'loanKinds': [
             {'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True}
         ],
         'KonsDbLoanType': 4},
        {'id': 20003, 'name': 'Hypoteční úvěr bez příjmu', 'mandantId': 2, 'loanAmountMin': 200000, 'loanAmountMax': 999999999, 'loanDurationMin': 5, 'loanDurationMax': 30, 'ltvMin': 0, 'ltvMax': 70, 'MpHomeApiLoanType': 'KBMortgageWithoutIncome',
         'loanKinds': [
             {'id': 2000, 'mandantId': 2, 'name': 'Standard', 'isDefault': True}
         ],
         'KonsDbLoanType': 5}
    ]


@pytest.mark.skip('Not implemented')
def test_codebook_riskApplicationTypes(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=riskApplicationTypes",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    print(response)
    codebook = response[0]['codebook']
    assert len(codebook) == 12
    assert sort_response_id(codebook) == []


def test_codebook_salesArrangementStates(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=SalesArrangementStates",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    response = resp.json()
    code = response[0]['code']
    codebook = response[0]['codebook']
    assert len(codebook) == 3
    assert code == 'SalesArrangementStates'
    assert sort_response_id(codebook) == [
        {"id": 1, "value": 1, "name": "Rozpracováno"},
        {"id": 2, "value": 2, "name": "Předáno ke zpracování"},
        {"id": 3, "value": 3, "name": "Zrušeno"}
    ]






