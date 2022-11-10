from Tests.backend.pytest.tests.noby_rest.db_util import select_product_konsdb_fat, select_offer_fat, select_create_case_fat


def test_update_db(noby_fat_db_cursor):
    noby_fat_db_cursor.execute("UPDATE CaseService.dbo.[Case] SET Name = 'Marek_db' WHERE CaseId=2954450")


def test_noby_db(noby_fat_db_cursor):
    noby_fat_db_cursor.execute("SELECT * FROM CaseService.dbo.[Case] WHERE CaseId=2954450")
    resp_query = noby_fat_db_cursor.fetchall()
    result = resp_query[0]
    print(result)
    case_id = result.CaseId #vytáhne z response selektu db caseId
    print(case_id)
    created_user_name = result.CreatedUserName
    assert created_user_name == 'Pavel Gronwaldt'
    print(created_user_name)


def test_kons_db(konsdb_fat_db_cursor):
    case_id = 2954450
    select_product_konsdb_fat(konsdb_fat_db_cursor, case_id)
    resp_query = konsdb_fat_db_cursor.fetchall()
    result = resp_query[0]
    id = result.Id
    print(id)
    print(result)


def test_noby_offer_db(noby_fat_db_cursor):
    offer_id = 338
    output = select_offer_fat(noby_fat_db_cursor, offer_id)
    db_id = output.OfferId
    assert offer_id == db_id


def test_dbv(konsdb_fat_db_cursor):
    case_id = 2954743
    output = select_product_konsdb_fat(konsdb_fat_db_cursor, case_id)
    db_id = output.Id
    print(output)
    print(db_id)
    assert case_id == db_id


def test_db_case(noby_fat_db_cursor):
    case_id = 2954744
    output = select_create_case_fat(noby_fat_db_cursor, case_id)
    print(output)
    db_CustomerOnSAId = output.CustomerOnSAId
    db_SalesArrangementId = output.SalesArrangementId
    db_CaseId = output.CaseId
    db_OfferId = output.OfferId
    db_HouseholdId = output.HouseholdId
    print(db_CustomerOnSAId)
    print(db_SalesArrangementId)
    print(db_CaseId)
    print(db_OfferId)
    print(db_HouseholdId)
    assert case_id == db_CaseId