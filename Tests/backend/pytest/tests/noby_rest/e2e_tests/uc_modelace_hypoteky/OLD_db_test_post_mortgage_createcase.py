import requests
from sqlalchemy import null
from Tests.backend.pytest.tests.noby_rest.db_util import select_offer_fat, select_create_case_fat, select_product_konsdb_fat
from Tests.backend.pytest.tests.noby_rest.construct_api.uc_modelace_hypoteky.post_offer_mortgage import post_offer_mortgage_loan_kind_2000, post_offer_mortgage_loan_kind_2001
from Tests.backend.pytest.tests.noby_rest.construct_api.uc_modelace_hypoteky.OLD_post_mortgage_createcase import post_create_case


# TODO: až bude modelace d1.3 hotova, dodelat test, nyní padá a dodělat constructAPI(je částečně hotové, ověřit)
def test_post_create_case(webapi_url, get_cookies, post_create_case, konsdb_fat_db_cursor, noby_fat_db_cursor):
    resp = post_create_case
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
