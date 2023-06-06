import pytest
from sqlalchemy import null
from Tests.backend.pytest.tests.noby_rest.db_util import select_offer_fat

from request.simulation_mortgage.OLD_simulation_mortgage_json import json_req_loan_kind_2000, json_req_loan_kind_2001


#TODO: příprava pro parametrizaci, dodělat x variant základních
@pytest.mark.parametrize("call_mortgage_json", [
    (
        json_req_loan_kind_2000
    ),
    (
        json_req_loan_kind_2001
    )]
)
def test_post_offer_mortgage(post_offer_mortgage):
    resp = post_offer_mortgage
    assert resp.status_code == 200, resp.content








#############OLD######################
# TODO: dodelat varianty modelace - ideal pres parametrizaci (lonaKind a podobne
def test_post_offer_mortgageloanKindId2000(webapi_url, post_offer_mortgage_loan_kind_2000, get_cookies, current_datetime_000z, tomorrow_datetime, noby_fat_db_cursor):
    resp = post_offer_mortgage_loan_kind_2000
    assert resp.status_code == 200, resp.content
    data = resp.json()
    fees = data['simulationResults']['fees']
    offer_id = data['offerId']
    value_loanTotalAmount = data['simulationResults']['loanTotalAmount']

    assert offer_id is not null
    assert fees is not null
    assert data['simulationResults'] == \
           {
               'loanAmount': 2000000,
               'loanDueDate': '2047-09-15T00:00:00',
               'loanDuration': 300,
               'loanPaymentAmount': 12270,
               'employeeBonusLoanCode': 201,
               'loanToValue': 66.67,
               'aprc': 5.67,
               'loanTotalAmount': value_loanTotalAmount,
               'loanInterestRateProvided': 5.49,
               'contractSignedDate': tomorrow_datetime,
               'drawingDateTo': '2022-10-14T00:00:00',
               'annuityPaymentsDateFrom': '2022-10-15T00:00:00',
               'annuityPaymentsCount': 300,
               'loanInterestRate': 5.49,
               'loanInterestRateAnnounced': 5.49,
               'loanInterestRateAnnouncedType': 1,
               'employeeBonusDeviation': 0,
               'marketingActionsDeviation': 0,
               'paymentScheduleSimple': [{'amount': '3\xa0965,00',
                                          'date': '15.9.2022',
                                          'paymentNumber': '1',
                                          'type': 'splátka úroků'},
                                         {'amount': '12\xa0270,00',
                                          'date': 'vždy k 15. dni měsíce',
                                          'paymentNumber': '2 - 300',
                                          'type': 'splátka'},
                                         {'amount': '12\xa0147,55',
                                          'date': '15.9.2047',
                                          'paymentNumber': '301',
                                          'type': 'splátka'}],
               'loanPurposes': [{'id': 202, 'sum': 1500000}, {'id': 204, 'sum': 500000}],
               'marketingActions':
                   [{'applied': False,
                     'code': 'DOMICILACE',
                     'deviation': 0,
                     'marketingActionId': 1,
                     'name': 'Domicilace (přirážka)',
                     'requested': True},
                    {'applied': False,
                     'code': 'RZP',
                     'deviation': 0,
                     'marketingActionId': 2,
                     'name': 'Rizikové životní pojištění (přirážka)',
                     'requested': True},
                    {'applied': False,
                     'code': 'POJIST_NEM',
                     'deviation': 0,
                     'marketingActionId': 4,
                     'name': 'Pojištění nemovitosti (sleva)',
                     'requested': False},
                    {'applied': False,
                     'code': 'VYSE_PRIJMU_UVERU',
                     'deviation': 0,
                     'marketingActionId': 3,
                     'name': 'Výše příjmů nebo úvěru (sleva)',
                     'requested': False},
                    {'applied': False,
                     'code': 'VIP_MAKLER',
                     'deviation': 0,
                     'marketingActionId': 20,
                     'name': 'VIP makléř (sleva)',
                     'requested': False},
                    {'applied': False,
                     'code': 'INDIVIDUALNI_SLEVA',
                     'deviation': 0,
                     'marketingActionId': 5,
                     'name': 'Individuální sleva poradce',
                     'requested': False}],
               'fees':
                   [{'feeId': 2001, 'discountPercentage': 0, 'tariffSum': 4900, 'composedSum': 0, 'finalSum': 4900,
                     'marketingActionId': 0, 'name': 'Zpracování žádosti',
                     'shortNameForExample': 'Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad',
                     'tariffName': 'Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad',
                     'usageText': 'VSM',
                     'tariffTextWithAmount': '4\xa0900 Kč', 'codeKB': 'FEE001', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': True,
                     'periodicity': 'I'},
                    {'feeId': 2002, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Čerpání úvěru', 'shortNameForExample': '', 'tariffName': 'Čerpání úvěru', 'usageText': 'S',
                     'tariffTextWithAmount': 'zdarma', 'codeKB': 'FEE034', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': False,
                     'periodicity': 'E'},
                    {'feeId': 2004, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Správa úvěru', 'shortNameForExample': 'Spravování úvěru (měsíčně)',
                     'tariffName': 'Spravování úvěru (měsíčně)', 'usageText': 'VS', 'tariffTextWithAmount': 'zdarma',
                     'codeKB': 'FEE003', 'displayAsFreeOfCharge': True, 'includeInRPSN': True, 'periodicity': 'M'},
                    {'feeId': 2005, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Výpis z úvěrového účtu', 'shortNameForExample': 'Výpis z úvěrového účtu (měsíčně)',
                     'tariffName': '',
                     'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE008', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': False, 'periodicity': 'M'},
                    {'feeId': 2006, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Poplatek za papírový výpis (úvěr)', 'shortNameForExample': '',
                     'tariffName': 'Služba zasílání výpisů z úvěrového účtu v papírové formě (měsíčně)', 'usageText': 'S',
                     'tariffTextWithAmount': 'zdarma', 'codeKB': 'FEE045', 'displayAsFreeOfCharge': True, 'includeInRPSN': True,
                     'periodicity': 'M'},
                    {'feeId': 2007, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Poplatek za elektronický výpis (úvěr)', 'shortNameForExample': '',
                     'tariffName': 'Služba zasílání výpisů z úvěrového účtu v elektronické formě (měsíčně)', 'usageText': 'S',
                     'tariffTextWithAmount': 'zdarma', 'codeKB': 'FEE035', 'displayAsFreeOfCharge': True, 'includeInRPSN': True,
                     'periodicity': 'M'},
                    {'feeId': 2010, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Vedení BÚ ke splácení', 'shortNameForExample': 'Vedení běžného účtu (měsíčně)', 'tariffName': '',
                     'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE009', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': True, 'periodicity': 'M'},
                    {'feeId': 2012, 'discountPercentage': 0, 'tariffSum': 1000, 'composedSum': 0, 'finalSum': 1000,
                     'marketingActionId': 0, 'name': 'Zpráva o stavu výstavby (1-3)', 'shortNameForExample': '',
                     'tariffName': 'Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)',
                     'usageText': 'S', 'tariffTextWithAmount': '1\xa0000 Kč první až třetí Zpráva,', 'codeKB': 'FEE031',
                     'displayAsFreeOfCharge': True, 'includeInRPSN': False, 'periodicity': 'E'},
                    {'feeId': 2013, 'discountPercentage': 0, 'tariffSum': 2900, 'composedSum': 0, 'finalSum': 2900,
                     'marketingActionId': 0, 'name': 'Zpráva o stavu výstavby (4 a více)', 'shortNameForExample': '',
                     'tariffName': 'Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)',
                     'usageText': 'S', 'tariffTextWithAmount': '2\xa0900 Kč čtvrtá a každá další Zpráva. ', 'codeKB': 'FEE084',
                     'displayAsFreeOfCharge': True, 'includeInRPSN': False, 'periodicity': 'E'},
                    {'feeId': 2015, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Výpis z BU ke splácení', 'shortNameForExample': 'Výpis z běžného účtu (měsíčně)',
                     'tariffName': '',
                     'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE036', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': True, 'periodicity': 'M'},
                    {'feeId': 2016, 'discountPercentage': 0, 'tariffSum': 2000, 'composedSum': 0, 'finalSum': 2000,
                     'marketingActionId': 0, 'name': 'Poplatek za vklad do KN',
                     'shortNameForExample': 'Návrh na vklad zástavního práva k nemovitostem do katastru nemovitostí',
                     'tariffName': '',
                     'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE037', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': True, 'periodicity': 'I'},
                    {'feeId': 2017, 'discountPercentage': 0, 'tariffSum': 2000, 'composedSum': 0, 'finalSum': 2000,
                     'marketingActionId': 0, 'name': 'Poplatek za výmaz z KN',
                     'shortNameForExample': 'Návrh na výmaz zástavního práva k nemovitostem z katastru nemovitostí',
                     'tariffName': '',
                     'usageText': 'V', 'tariffTextWithAmount': '', 'codeKB': 'FEE079', 'displayAsFreeOfCharge': True,
                     'includeInRPSN': True, 'periodicity': 'C'},
                    {'feeId': 2032, 'discountPercentage': 0, 'tariffSum': 0, 'composedSum': 0, 'finalSum': 0,
                     'marketingActionId': 0,
                     'name': 'Změna smlouvy GREC', 'shortNameForExample': 'Změna ve smlouvě', 'tariffName': '',
                     'usageText': 'V',
                     'tariffTextWithAmount': '', 'codeKB': 'FEE078', 'displayAsFreeOfCharge': True, 'includeInRPSN': True,
                     'periodicity': 'I'}]
        }
    # kontrola v databazi
    output = select_offer_fat(noby_fat_db_cursor, offer_id)
    db_id = output.OfferId
    assert offer_id == db_id


# TODO: dodelat varianty modelace - ideal pres parametrizaci (lonaKind a podobne
def test_post_offer_mortgage_loanKindId2001(webapi_url, get_cookies, post_offer_mortgage_loan_kind_2001, current_datetime_000z, tomorrow_datetime, noby_fat_db_cursor):
    resp = post_offer_mortgage_loan_kind_2001
    assert resp.status_code == 200, resp.content
    data = resp.json()
    fees = data['fees']
    offer_id = data['offerId']
    value_loanTotalAmount = data['simulationResults']['loanTotalAmount']
    assert offer_id is not null
    assert fees is not null
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
                       {'code': 'INDIVIDUALNI_SLEVA', 'requested': False, 'applied': False}]
           }
    assert fees == \
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
    # kontrola v databazi
    output = select_offer_fat(noby_fat_db_cursor, offer_id)
    db_id = output.OfferId
    assert offer_id == db_id
