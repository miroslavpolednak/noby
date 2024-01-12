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
    json_req_mail_mpss_sender_mpss, json_req_mail_mpss_sender_vsskb, json_req_mail_mpss_sender_mpss_info

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal,
                                       json_req_mail_mpss_basic_natural,
                                       json_req_mail_kb_basic_legal])
def test_mail(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# základní test
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural])
def test_mail_full(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [
    json_req_mail_mpss_basic_content_format_application_mht,
])
def test_mail_content_format(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""

    # Pro druhý GET request potřebujete přihlašovací údaje NOBY uživatele
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]

    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(noby_username, noby_password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    valid_states = ['Sent', 'InProgress']
    assert resp['state'] in valid_states, f"Invalid state: {resp['state']}, expected one of {valid_states}"
    assert resp['channel'] == 'Email'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    time.sleep(15)

    # vola GET opet, abz si overil doruceni
    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(noby_username, noby_password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    assert resp['state'] == 'Sent'


# test variant
@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_max_attachments,
                                       json_req_mail_kb_max_attachments])
def test_mail_max_attachments(ns_url, auth_params, auth, json_data):
    """max priloh klady test"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_null_party_from,
                                       json_req_mail_mpss_without_party_from]
                         )
def test_mail_party(auth_params, auth, json_data, ns_url):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# základní test case id
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_case])
def test_mail_caseId(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# základní test documentHash
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_documentHash_SHA_256,
                                       json_req_mail_mpss_documentHash_SHA_3,
                                       json_req_mail_mpss_documentHash_SHA_512,
                                       json_req_mail_mpss_documentHash_SHA_384],
                         ids=[
                             "json_req_mail_mpss_documentHash_SHA_256",
                             "json_req_mail_mpss_documentHash_SHA_3",
                             "json_req_mail_mpss_documentHash_SHA_512",
                             "json_req_mail_mpss_documentHash_SHA_384"
                         ]
                         )
def test_mail_documentHash(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# zatím bez json_req_mail_mpss_sender_vsskb a json_req_mail_kb_sender_kb_sluzby
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_sender_mpss, json_req_mail_mpss_sender_mpss_info,
                                       json_req_mail_kb_sender_kb, json_req_mail_kb_sender_kb_sluzby,
                                       json_req_mail_kb_sender_kbinfo,
                                       json_req_mail_kb_sender_kb_attachment
                                       ],
                         ids=[
                             "json_req_mail_mpss_sender_mpss",
                             "json_req_mail_mpss_sender_mpss_info",
                             "json_req_mail_kb_sender_kb",
                             "json_req_mail_kb_sender_kb_sluzby",
                             "json_req_mail_kb_sender_kbinfo",
                             "json_req_mail_kb_sender_kb_attachment"
                         ]
                         )
def test_mail_sender(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# základní test
# @pytest.mark.skip(reason="pro ruční spouštění")
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("fat_url", json_req_mail_mpss_full_attachments)
])
def test_mail_for_resend(url_name, auth_params, auth, json_data, mssql_connection):
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

    ##################### Kontrola, že v SentNotification není
    table_name = 'SentNotification'
    schema_name = 'dbo'

    cursor.execute(f"""
                SELECT *
                FROM {schema_name}.{table_name}
                WHERE Id = ?;
            """, (notification_id,))
    # Získání záznamu
    row = cursor.fetchone()
    # Ověření, že záznam existuje, a výpis jeho obsahu
    if row is not None:
        print("Nalezený záznam:")
        for column_value in row:
            print(column_value)
    else:
        print(f"Záznam s ID {notification_id} nebyl nalezen.")

    # provolání resend - může pouze NOBY user
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/resend/{notification_id}",
        auth=(noby_username, noby_password),
        verify=False
    )
    assert resp.status_code == 200

    ##Ověření opětovného inProgress
    table_name = 'EmailResult'
    schema_name = 'dbo'
    select_query = f"""
                SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
                WHERE Id = ?;
            """
    cursor.execute(select_query, (notification_id,))
    resend_row = cursor.fetchone()
    print(f"result po zahájení resend{resend_row}")

    assert resend_row[1] == 1, f"Resend: Očekávaný State je 1 IN PROGRESS, ale získaný State je {row[1]}"
    assert resend_row[2] == True, f"Resend: Očekávaný resent je true, ale získaný resent je {row[2]}"

    time.sleep(61)

    ##################### Kontrola, že v SentNotification je zamčen
    table_name = 'SentNotification'
    schema_name = 'dbo'

    cursor.execute(f"""
                    SELECT *
                    FROM {schema_name}.{table_name}
                    WHERE Id = ?;
                """, (notification_id,))
    # Získání záznamu
    row = cursor.fetchone()
    # Ověření, že záznam existuje, a výpis jeho obsahu
    if row is not None:
        print("Nalezený záznam:")
        for column_value in row:
            print(column_value)
    else:
        print(f"Záznam s ID {notification_id} nebyl nalezen.")
    # konrola je byl odeslán a ja zamknuto
    assert row is not None

    ##Ověření odeslání
    table_name = 'EmailResult'
    schema_name = 'dbo'
    select_query = f"""
                    SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
                    WHERE Id = ?;
                """
    cursor.execute(select_query, (notification_id,))
    resend_row = cursor.fetchone()
    print(f"result po konec resend{resend_row}")

    assert resend_row[1] == 3, f"Resent: Očekávaný State je 3 SEND, ale získaný State je {row[1]}"
    assert resend_row[2] == False, f"Resent: Očekávaný resent je false, ale získaný resent je {row[2]}"


# @pytest.mark.skip("manual")
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("fat_url", json_req_mail_mpss_full_attachments)
])
def test_mail_jobs(url_name, auth_params, auth, json_data, mssql_connection):
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

    ##Ověření opětovného inProgress
    table_name = 'EmailResult'
    schema_name = 'dbo'
    select_query = f"""
                SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
                WHERE Id = ?;
            """
    cursor.execute(select_query, (notification_id,))
    row = cursor.fetchone()
    print(f"result po zahájení{row}")

    assert row[1] == 1, f"Očekávaný State je 1 IN PROGRESS, ale získaný State je {row[1]}"
    assert row[2] == False, f"Očekávaný resent je false, ale získaný resent je {row[2]}"
    assert row[3] == 2, f"Očekávaný Serner je MPSS, ale získaný resent je {row[2]}"

    time.sleep(61)

    ##################### Kontrola, že v SentNotification je zamčen
    table_name = 'SentNotification'
    schema_name = 'dbo'

    cursor.execute(f"""
                    SELECT *
                    FROM {schema_name}.{table_name}
                    WHERE Id = ?;
                """, (notification_id,))
    # Získání záznamu
    row = cursor.fetchone()
    # Ověření, že záznam existuje, a výpis jeho obsahu
    if row is not None:
        print("Nalezený záznam:")
        for column_value in row:
            print(column_value)
    else:
        print(f"Záznam s ID {notification_id} nebyl nalezen.")
    # konrola je byl odeslán a ja zamknuto
    assert row is not None

    ##Ověření odeslání
    table_name = 'EmailResult'
    schema_name = 'dbo'
    select_query = f"""
                    SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
                    WHERE Id = ?;
                """
    cursor.execute(select_query, (notification_id,))
    resend_row = cursor.fetchone()
    print(f"result po konci jobu{resend_row}")

    assert resend_row[1] == 3, f"Resent: Očekávaný State je 3 SEND, ale získaný State je {row[1]}"
    assert resend_row[2] == False, f"Resent: Očekávaný resent je false, ale získaný resent je {row[2]}"


@pytest.mark.skip("pro testy connection do databaze")
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
def test_connection(mssql_connection):
    """
        Test pro ověření, zda datová struktura tabulky 'SalesArrangementParameters'
        v databázi odpovídá očekávané definici. Kontroluje datové typy, maximální
        délky a nullable atributy sloupců.
        """
    cursor = mssql_connection.cursor()
    table_name = 'SendEmail'
    schema_name = 'DDS'  # Název schématu

    # Dotaz na získání informací o sloupcích v tabulce
    cursor.execute(f"""
        SELECT DocumentDataEntityId, Data, CreatedTime
        FROM {schema_name}.{table_name}
        """)
    columns = cursor.fetchall()

    # Pro ilustraci vypíše informace o sloupcích
    for col in columns:
        print(
            f"NSid: {col.DocumentDataEntityId}")
