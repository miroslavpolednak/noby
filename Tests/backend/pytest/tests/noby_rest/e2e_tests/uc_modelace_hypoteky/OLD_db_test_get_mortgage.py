import requests
from sqlalchemy import null
from Tests.backend.pytest.tests.noby_rest.db_util import select_offer_fat, select_create_case_fat, select_product_konsdb_fat, select_partner_konsdb_fat
from Tests.backend.pytest.tests.noby_rest.construct_api.uc_modelace_hypoteky.post_offer_mortgage import post_offer_mortgage_loan_kind_2000, post_offer_mortgage_loan_kind_2001
from Tests.backend.pytest.tests.noby_rest.construct_api.uc_modelace_hypoteky.get_offer_mortgage_offerid import get_offer_mortgage_loan_kind_2000


def test_get_offer_mortgage_loanKindId2000(webapi_url, get_cookies, get_offer_mortgage_loan_kind_2000, tomorrow_datetime,
                                           guarantee_date_from_datetime):
    resp = get_offer_mortgage_loan_kind_2000
    assert resp.status_code == 200, resp.content
    offer_id = resp[0]
    resp = resp[1]
    data = resp.json()
    value_loanTotalAmount = data['simulationResults']['loanTotalAmount']
    assert data['offerId'] == offer_id
    assert data['fees'] == \
           [
               {'feeId': 2001, 'discountPercentage': 0, 'tariffSum': 4900, 'composedSum': 0, 'finalSum': 4900,
                'marketingActionId': 0, 'name': 'Zpracování žádosti',
                'shortNameForExample': 'Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad',
                'tariffName': 'Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad',
                'usageText': 'VSM', 'tariffTextWithAmount': '4\xa0900 Kč', 'codeKB': 'FEE001',
                'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'I'},
               {'feeId': 2002, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Čerpání úvěru', 'shortNameForExample': '',
                'tariffName': 'Čerpání úvěru', 'usageText': 'S', 'tariffTextWithAmount': 'zdarma', 'codeKB': 'FEE034',
                'displayAsFreeOfCharge': True, 'includeInRPSN': False, 'periodicity': 'E'},
               {'feeId': 2004, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Správa úvěru', 'shortNameForExample': 'Spravování úvěru (měsíčně)',
                'tariffName': 'Spravování úvěru (měsíčně)', 'usageText': 'VS', 'tariffTextWithAmount': 'zdarma',
                'codeKB': 'FEE003', 'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'M'},
               {'feeId': 2005, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Výpis z úvěrového účtu',
                'shortNameForExample': 'Výpis z úvěrového účtu (měsíčně)', 'tariffName': '', 'usageText': 'V',
                'tariffTextWithAmount': '', 'codeKB': 'FEE008', 'displayAsFreeOfCharge': True, 'includeInRPSN': False,
                'periodicity': 'M'},
               {'feeId': 2006, 'discountPercentage': 0, 'tariffSum': 60, 'composedSum': 0, 'finalSum': 60,
                'marketingActionId': 0, 'name': 'Poplatek za papírový výpis (úvěr)', 'shortNameForExample': '',
                'tariffName': 'Služba zasílání výpisů z úvěrového účtu v papírové formě (měsíčně)', 'usageText': 'S',
                'tariffTextWithAmount': '60 Kč', 'codeKB': 'FEE045', 'displayAsFreeOfCharge': True,
                'includeInRPSN': True, 'periodicity': 'M'},
               {'feeId': 2007, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Poplatek za elektronický výpis (úvěr)', 'shortNameForExample': '',
                'tariffName': 'Služba zasílání výpisů z úvěrového účtu v elektronické formě (měsíčně)',
                'usageText': 'S', 'tariffTextWithAmount': 'zdarma', 'codeKB': 'FEE035', 'displayAsFreeOfCharge': True,
                'includeInRPSN': True, 'periodicity': 'M'},
               {'feeId': 2010, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Vedení BÚ ke splácení',
                'shortNameForExample': 'Vedení běžného účtu (měsíčně)', 'tariffName': '', 'usageText': 'V',
                'tariffTextWithAmount': '', 'codeKB': 'FEE009', 'displayAsFreeOfCharge': True, 'includeInRPSN': True,
                'periodicity': 'M'},
               {'feeId': 2012, 'discountPercentage': 0, 'tariffSum': 1000, 'composedSum': 0, 'finalSum': 1000,
                'marketingActionId': 0, 'name': 'Zpráva o stavu výstavby (1-3)', 'shortNameForExample': '',
                'tariffName': 'Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)',
                'usageText': 'S', 'tariffTextWithAmount': '1\xa0000 Kč první až třetí Zpráva,', 'codeKB': 'FEE031',
                'displayAsFreeOfCharge': True, 'includeInRPSN': False, 'periodicity': 'E'},
               {'feeId': 2013, 'discountPercentage': 0, 'tariffSum': 2900, 'composedSum': 0, 'finalSum': 2900,
                'marketingActionId': 0, 'name': 'Zpráva o stavu výstavby (4 a více)', 'shortNameForExample': '',
                'tariffName': 'Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)',
                'usageText': 'S', 'tariffTextWithAmount': '2\xa0900 Kč čtvrtá a každá další Zpráva. ',
                'codeKB': 'FEE084', 'displayAsFreeOfCharge': True, 'includeInRPSN': False, 'periodicity': 'E'},
               {'feeId': 2015, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Výpis z BU ke splácení',
                'shortNameForExample': 'Výpis z běžného účtu (měsíčně)', 'tariffName': '', 'usageText': 'V',
                'tariffTextWithAmount': '', 'codeKB': 'FEE036', 'displayAsFreeOfCharge': True, 'includeInRPSN': True,
                'periodicity': 'M'},
               {'feeId': 2016, 'discountPercentage': 0, 'tariffSum': 2000, 'composedSum': 0, 'finalSum': 2000,
                'marketingActionId': 0, 'name': 'Poplatek za vklad do KN',
                'shortNameForExample': 'Návrh na vklad zástavního práva k nemovitostem do katastru nemovitostí',
                'tariffName': '', 'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE037',
                'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'I'},
               {'feeId': 2017, 'discountPercentage': 0, 'tariffSum': 2000, 'composedSum': 0, 'finalSum': 2000,
                'marketingActionId': 0, 'name': 'Poplatek za výmaz z KN',
                'shortNameForExample': 'Návrh na výmaz zástavního práva k nemovitostem z katastru nemovitostí',
                'tariffName': '', 'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE079',
                'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'C'},
               {'feeId': 2032, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                'marketingActionId': 0, 'name': 'Změna smlouvy GREC', 'shortNameForExample': 'Změna ve smlouvě',
                'tariffName': '', 'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE078',
                'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'I'}]
    assert data['simulationInputs'] == \
           {'productTypeId': 20001,
            'loanKindId': 2000,
            'loanAmount': 2000000,
            'loanDuration': 300,
            'fixedRatePeriod': 60,
            'collateralAmount': 3000000,
            'paymentDay': 15,
            'isEmployeeBonusRequested': False,
            'expectedDateOfDrawing': tomorrow_datetime,
            'financialResourcesOwn': 600000,
            'financialResourcesOther': 400000,
            'guaranteeDateFrom': guarantee_date_from_datetime,
            'drawingType': 0,
            'loanPurposes': [{'id': 202, 'sum': 1500000}, {'id': 204, 'sum': 500000}]}
    assert data['simulationResults'] == \
           {
               'loanAmount': 2000000,
               'loanDueDate': '2047-08-15T00:00:00',
               'loanDuration': 300,
               'loanPaymentAmount': 12270,
               'employeeBonusLoanCode': 201,
               'loanToValue': 66.67,
               'aprc': 5.67,
               'loanTotalAmount': value_loanTotalAmount,
               'loanInterestRateProvided': 5.49,
               'contractSignedDate': tomorrow_datetime,
               'drawingDateTo': '2022-09-14T00:00:00',
               'annuityPaymentsDateFrom': '2022-09-15T00:00:00',
               'annuityPaymentsCount': 300,
               'loanInterestRate': 5.49,
               'loanInterestRateAnnounced': 5.49,
               'loanInterestRateAnnouncedType': 1,
               'employeeBonusDeviation': 0,
               'marketingActionsDeviation': 0,
               'loanPurposes': [{'id': 202, 'sum': 1500000}, {'id': 204, 'sum': 500000}],
               'marketingActions':
                   [
                       {'code': 'DOMICILACE', 'requested': True, 'applied': False},
                       {'code': 'RZP', 'requested': True, 'applied': False},
                       {'code': 'POJIST_NEM', 'requested': False, 'applied': False},
                       {'code': 'VYSE_PRIJMU_UVERU', 'requested': False, 'applied': False},
                       {'code': 'VIP_MAKLER', 'requested': False, 'applied': False},
                       {'code': 'INDIVIDUALNI_SLEVA', 'requested': False, 'applied': False}],
               'paymentDay': 15
           }


# TODO: asserty
def test_post_create_case(webapi_url, get_cookies, post_offer_mortgage_loan_kind_2000, konsdb_fat_db_cursor, noby_fat_db_cursor):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage/create-case",
        cookies=get_cookies,
        json=
        {"offerId": post_offer_mortgage_loan_kind_2000,
         "firstName": "KLIENT",
         "lastName": "TESTOVACI",
         "dateOfBirth": "1967-08-30T00:00:00",
         "identity": {"id": 113666423, "scheme": 2}}
    )
    case = resp.json()
    assert resp.status_code == 200, resp.content
    print(case)
    case_id = case['caseId']
    salesArrangement_id = case['salesArrangementId']
    offer_id = case['offerId']
    household_id = case['householdId']
    customerOnSA_id = case['customerOnSAId']

    # kontrola v databazi konsdb zalozeni productId
    output = select_product_konsdb_fat(konsdb_fat_db_cursor, case_id)
    db_id = output.Id
    assert case_id == db_id

    output = select_partner_konsdb_fat(konsdb_fat_db_cursor, rodne_cislo_ico)
    #dodelat vytah

    #kotrola v NOBY databazi
    output = select_create_case_fat(noby_fat_db_cursor, case_id)
    db_customerOnSAId = output.CustomerOnSAId
    db_salesArrangementId = output.SalesArrangementId
    db_caseId = output.CaseId
    db_offerId = output.OfferId
    db_householdId = output.HouseholdId

    assert salesArrangement_id == db_salesArrangementId
    assert offer_id == db_offerId
    assert household_id == db_householdId
    assert customerOnSA_id == db_customerOnSAId
    assert case_id == db_caseId
