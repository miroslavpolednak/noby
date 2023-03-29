
# selekty do databazi
def select_product_konsdb_fat(konsdb_fat_db_cursor, case_id):
    konsdb_fat_db_cursor.execute("SELECT * FROM KonsDb_L1_HFFAT.dbo.Uver n WHERE n.id = ?", case_id)
    resp_query = konsdb_fat_db_cursor.fetchall()
    result = resp_query[0]
    return result

def select_partner_konsdb_fat(konsdb_fat_db_cursor, rodne_cislo_ico):
    konsdb_fat_db_cursor.execute("SELECT * FROM KonsDb_L1_HFFAT.dbo.Partner n WHERE n.RodneCisloIco = ?", rodne_cislo_ico)
    resp_query = konsdb_fat_db_cursor.fetchall()
    result = resp_query[0]
    return result

def select_offer_fat(noby_fat_db_cursor, offer_id):
    noby_fat_db_cursor.execute("SELECT * FROM OfferService.dbo.Offer n WHERE n.OfferId = ?", offer_id)
    resp_query = noby_fat_db_cursor.fetchall()
    result = resp_query[0]
    return result


def select_create_case_fat(noby_fat_db_cursor, case_id):
    noby_fat_db_cursor.execute("SELECT * FROM SalesArrangementService.dbo.CustomerOnSA csa \
JOIN SalesArrangementService.dbo.SalesArrangement sa ON (csa.SalesArrangementId=sa.SalesArrangementId) \
JOIN SalesArrangementService.dbo.Household h ON (csa.SalesArrangementId=h.SalesArrangementId) \
JOIN CaseService.dbo.[Case] c ON (sa.CaseId=c.CaseId) \
WHERE c.CaseId = ?", case_id)
    resp_query = noby_fat_db_cursor.fetchall()
    result = resp_query[0]
    return result