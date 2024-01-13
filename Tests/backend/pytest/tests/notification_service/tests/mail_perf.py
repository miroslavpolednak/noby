import sqlite3
import time

import pytest
import requests
import urllib3
from sqlalchemy.dialects.mssql.information_schema import columns
from sqlalchemy.orm import query

from Tests.backend.pytest.tests.notification_service.conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_max_attachments, json_req_mail_kb_basic_legal, \
    json_req_mail_kb_sender_kb, json_req_mail_kb_sender_kb_attachment, json_req_mail_kb_basic_format_application_html, \
    json_req_mail_kb_basic_content_format_application_mht, json_req_mail_kb_basic_format_application_text, \
    json_req_mail_kb_basic_format_html, json_req_mail_kb_basic_format_text_html, \
    json_req_mail_kb_basic_format_text_plain, json_req_mail_kb_sender_kbinfo, json_req_mail_kb_sender_kb_sluzby
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, \
    json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural, \
    json_req_mail_mpss_basic_format_html, \
    json_req_mail_mpss_basic_format_text_html, json_req_mail_mpss_basic_format_application_html, \
    json_req_mail_mpss_basic_content_format_application_mht, json_req_mail_mpss_null_party_from, \
    json_req_mail_mpss_without_party_from, json_req_mail_mpss_case, \
    json_req_mail_mpss_documentHash_SHA_256, json_req_mail_mpss_documentHash_SHA_3, \
    json_req_mail_mpss_documentHash_SHA_512, json_req_mail_mpss_documentHash_SHA_384, \
    json_req_mail_mpss_basic_format_text_plain, json_req_mail_mpss_basic_format_application_text, \
    json_req_mail_mpss_max_attachments, \
    json_req_mail_mpss_sender_mpss, json_req_mail_mpss_sender_vsskb, json_req_mail_mpss_sender_mpss_info, \
    json_req_mail_mpss_sender_modrapyramida, json_req_mail_mpss_sender_mpssinfo
collected_notification_ids = []
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

@pytest.fixture(scope="session", autouse=True)
def collect_notification_ids():
    global collected_notification_ids
    yield
    # Ukládání do SQLite databáze po dokončení všech testů
    try:
        conn = sqlite3.connect('test_results.db')
        c = conn.cursor()
        c.execute('''CREATE TABLE IF NOT EXISTS notification_ids (id TEXT)''')
        for notification_id in collected_notification_ids:
            c.execute("INSERT INTO notification_ids VALUES (?)", (notification_id,))
        conn.commit()
    except Exception as e:
        print("Chyba při práci s databází:", e)
    finally:
        conn.close()


@pytest.mark.parametrize("run_number", range(2))  # Spustí test 10x
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("fat_url", json_req_mail_mpss_full_attachments)
])
def test_mail_for_perf(run_number, url_name, auth_params, auth, json_data, mssql_connection):
    """kladny test"""
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    # Uložení notificationId do globálního seznamu
    global collected_notification_ids
    collected_notification_ids.append(notification_id)
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'

    cursor = mssql_connection.cursor()
    ###########################kotrola v SendEmail uložení Payloadu
    table_name = 'SendEmail'
    schema_name = 'DDS'  # Název schématu
    cursor.execute(f"""
            SELECT DocumentDataEntityId, Data, CreatedTime
            FROM {schema_name}.{table_name}
            WHERE DocumentDataEntityId = ?;
            """, (notification_id,)
                   )
    columns = cursor.fetchall()
    # Assert, že byl nalezen alespoň jeden záznam
    assert len(columns) > 0, "Žádný záznam nebyl nalezen"
    # Assert, že sloupec Data má hodnotu
    # Předpokládá se, že 'Data' je druhý sloupec v dotazu
    assert columns[0][1] is not None and columns[0][1] != "", "Sloupec Data je prázdný nebo None"
    # Pro ilustraci vypíše informace o sloupcích
    for col in columns:
        print(
            f"NSid: {col.DocumentDataEntityId}")

        ######################## update do db na state 2 (Unsent
        # Definice názvu tabulky a schématu
        table_name = 'EmailResult'
        schema_name = 'dbo'
        # Parametrizovaný SQL příkaz pro aktualizaci
        cursor.execute(f"""
            UPDATE {schema_name}.{table_name}
            SET State = 2
            WHERE Id = ?;
        """, (notification_id,))

        # Potvrzení změn v databázi
        mssql_connection.commit()

        ##Ověření provedení změn pomocí SELECT dotazu
        select_query = f"""
            SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
            WHERE Id = ?;
        """
        cursor.execute(select_query, (notification_id,))
        row = cursor.fetchone()
        print(row)

        assert row[1] == 2, f"Očekávaný State je 2 UNSENT, ale získaný State je {row[1]}"
