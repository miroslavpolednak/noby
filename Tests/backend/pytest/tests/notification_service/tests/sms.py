import uuid
import datetime
from time import sleep
from urllib.parse import urlencode, quote

"""
jak provolat s upravou additional argument, ze složky: notification_service> pytest .\tests\sms.py --ns-url fat_url --db-url fat_db
"""
import pyodbc
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

import pytest
import requests

from ..conftest import URLS, greater_than_zero, ns_url, auth
from ..json.request.seg_log import json_req_basic_log
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb_E2E, json_req_sms_sb, \
    json_req_sms_bad_basic_without_identifier, json_req_sms_bad_basic_without_identifier_scheme, \
    json_req_sms_bad_basic_without_identifier_identity, json_req_sms_basic_insg_uat, json_req_sms_mpss_archivator, \
    json_req_sms_kb_archivator, json_req_sms_basic_insg_fat, json_req_sms_basic_insg_sit, json_req_sms_basic_insg_e2e, \
    json_req_sms_caseId, json_req_sms_documentHash, json_req_sms_basic_kb_insg, json_req_sms_logovani_mpss_sb, \
    json_req_sms_bez_logovani_mpss_sb, json_req_sms_logovani_kb_insg
from ..json.request.sms_template_json import json_req_sms_full_template


# test pro additional parameters napr. --ns-url sit_url


@pytest.mark.skip(reason="pro ruční spouštění")
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("dev_url", json_req_sms_basic_insg),
    ("fat_url", json_req_sms_basic_insg_fat),
    ("sit_url", json_req_sms_basic_insg_sit),
    ("uat_url", json_req_sms_basic_insg_uat)
])
def test_sms_manualy(url_name, auth_params, auth, json_data):
    """
    uvodni test pro zakladni napln sms bez priloh, pro ruční spouštění
    """
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# @pytest.mark.skip(reason="real")
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg_e2e])
def test_E2E_real_sms(ns_url, auth_params, auth, json_data, modified_json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    # Přidáváme hodnotu ns_url do JSON dat
    json_data['text'] = json_data['text'] + " " + url_name

    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=modified_json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# test pro additional parameters napr. --ns-url sit_url
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg])
def test_sms_insg(ns_url, auth_params, auth, json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
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


@pytest.mark.parametrize("auth", ["XX_KBINSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_kb_insg])
def test_sms_kb_insg(ns_url, auth_params, auth, json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
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


@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_sb])
def test_sms_sb(ns_url, auth_params, auth, json_data):
    """
    SB test do budoucna, který ale ještě nemá svůj mcs type sms
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# TODO: username pro archivator získat od Karla až bude
@pytest.mark.skip(reason="SB test do budoucna, který ale ještě nemá svůj mcs type sms")
@pytest.mark.parametrize("auth, json_data", [
    ("XX_INSG_RMT_USR_TEST", json_req_sms_mpss_archivator),
    ("XX_INSG_RMT_USR_TEST", json_req_sms_kb_archivator),
], indirect=["auth"])
def test_sms_archivator(ns_url, auth_params, auth, json_data):
    """
    SB test pro archivaci
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""

@pytest.mark.skip("Nefunguje dotaz do databáze")
@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("custom_id, json_data, expected_result", [
    ("loguji", json_req_sms_logovani_kb_sb_E2E, True)
])
def test_sms_log(ns_url, auth_params, auth, custom_id, json_data,
                 expected_result, db_url, db_connection):
    """test logovani - zalogujeme, nezalogujeme do seq"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    # vytvoření unikátního ID pro customId pro dohledani logu
    uuid_str = uuid.uuid4().hex
    unique_custom_id = f"{uuid_str[:4]}{custom_id}{uuid_str[4:8]}"

    # upravit json_data před požadavkem
    json_data['customId'] = unique_custom_id

    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""
    print(datetime.datetime.now())

    cursor = db_connection.cursor()
    print(f"Notification ID: {notification_id}")
    print(f"custom ID: {unique_custom_id}")
    # notification_id_db = str(resp["notificationId"])

    print("--------PRVNÍ LOG NOBY_013-----------")
    sleep(3)
    print(f"Executing query with notification ID: {notification_id} and AuditEventTypeId: 'AU_NOBY_013'")
    try:
        cursor.execute("""
            SELECT * 
            FROM NobyAudit.dbo.AuditEvent
            CROSS APPLY OPENJSON(JSON_VALUE(Detail, '$.body.objectsAfter'))
            WITH (
            notificationId VARCHAR(100) '$.notificationId'
            )
            WHERE notificationId = ? AND AuditEventTypeId = ?;
            """, (notification_id, 'AU_NOBY_013')
                       )
        results_1 = cursor.fetchall()
        print(f"Query results: {results_1}")
        found_records_1 = bool(results_1)
        assert found_records_1 == expected_result
    except pyodbc.Error as e:
        pytest.fail(f"Failed to execute query 1: {e}")

    print(f'(%\\\"customId\\\": \\\"{unique_custom_id}\\\"%)')
    sleep(5)

    print("--------DRUHÝ LOG NOBY_012-----------")
    try:
        like_pattern = f'%{unique_custom_id}%'
        query = """
                SELECT * 
                FROM AuditEvent
                WHERE AuditEventTypeId = ? 
                AND Detail LIKE ?
                ORDER BY [TimeStamp] DESC
                """
        print("SQL Query:", query)
        print("Parameters:", ('AU_NOBY_012', like_pattern))
        cursor.execute(query, ('AU_NOBY_012', like_pattern))
        results_2 = cursor.fetchall()
        found_records_2 = bool(results_2)
        print("Results:", results_2)
        assert found_records_2 == expected_result
    except pyodbc.Error as e:
        pytest.fail(f"Failed to execute query 2: {e}")

    print("--------TŘETÍ LOG NOBY_014-----------")
    try:
        cursor.execute("""
                     SELECT * 
                     FROM AuditEvent
                     CROSS APPLY OPENJSON(JSON_VALUE(Detail, '$.body.objectsBefore'))
                     WITH (
                     notificationId VARCHAR(100) '$.notificationId'
                     )
                     WHERE notificationId = ? AND AuditEventTypeId = ?;
                     """, (notification_id, 'AU_NOBY_014')
                       )
        results_3 = cursor.fetchall()
        found_records_3 = bool(results_3)
        assert found_records_3 == expected_result
    except pyodbc.Error as e:
        pytest.fail(f"Failed to execute query 3: {e}")


"""
    sleep(2)

    # Kontrola v seq logu
    params = {
        "filter": f"@Message like '%{unique_custom_id}%' ci or @Exception like '%{unique_custom_id}%' ci or @Message like '%{notification_id}%' ci or @Exception like '%{notification_id}%' ci",
        "shortCircuitAfter": 100}

    encoded_params = "&".join(f"{key}={quote(str(value), safe='')}" for key, value in params.items())
    url = f"https://172.30.35.51:6341/api/events/signal?{encoded_params}"

    resp = authenticated_seqlog_session.post(
        url,
        json=json_req_basic_log
    )
    print(resp.json())

    events = resp.json()['Events']
    result = any(
        any(prop["Name"] == "SmsType" for prop in event["Properties"]) for event in
        events if "Properties" in event)

    if expected_result:
        assert any(
            any(prop["Name"] == "SmsType" for prop in event["Properties"]) for event in
            events if
            "Properties" in event), f"Failed for custom_id: {custom_id}. Expected: {expected_result}, Got: {result}"
    else:
        assert all(
            not any(prop["Name"] == "SmsType" for prop in event["Properties"]) for event in
            events if
            "Properties" in event), f"Failed for custom_id: {custom_id}. Expected: {expected_result}, Got: {result}"
"""


# NOBY vraci okej, ale v databázi padnou kombinace, ktere nemaji pro sebe MCS kod
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data",
                         [json_req_sms_basic_insg, json_req_sms_basic_epsy_kb, json_req_sms_logovani_kb_sb_E2E])
def test_sms_combination_security(ns_url, auth_params, auth, json_data):
    """test pro kombinaci všech uživatelů s basic sms"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    notification_id = resp["notificationId"]
    assert "notificationId" in resp
    assert notification_id != ""


@pytest.mark.parametrize("auth, json_data",
                         [
                             ("XX_INSG_RMT_USR_TEST", json_req_sms_basic_insg),
                             ("XX_EPSY_RMT_USR_TEST", json_req_sms_basic_epsy_kb),
                             ("XX_SB_RMT_USR_TEST", json_req_sms_logovani_kb_sb_E2E),
                         ], indirect=["auth"])
def test_sms_basic_security(auth_params, auth, json_data, ns_url):
    """
   Testuje zabezpečení: kladné testy pro usery a jejich msc type kody.
   Použita vnorena parametrizace pro různé uživatele a type sms.
   """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    assert resp["notificationId"] != ""


@pytest.mark.skip("Noby má jespíše práva - ověřit")
@pytest.mark.parametrize("auth, json_data",
                         [
                             ("XX_NOBY_RMT_USR_TEST", json_req_sms_basic_insg)
                         ], indirect=["auth"])
def test_sms_bad_basic_security(ns_url, auth_params, auth, json_data):
    """
   Testuje zabezpečení: kladné testy pro usery a jejich msc type kody.
   Použita vnorena parametrizace pro různé uživatele a type sms.
   """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 403


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data, expected_error", [
    (json_req_sms_bad_basic_without_identifier, {
        'Identifier.Identity': ['The Identity field is required.'],
        'Identifier.IdentityScheme': ['The IdentityScheme field is required.']
    }),
    (json_req_sms_bad_basic_without_identifier_scheme, {
        'Identifier.IdentityScheme': ['The IdentityScheme field is required.']
    }),
    (json_req_sms_bad_basic_without_identifier_identity, {
        'Identifier.Identity': ['The Identity field is required.']
    })
])
def test_sms_bad_identifier(ns_url, auth_params, auth, json_data, expected_error):
    """uvodni errors test pro zakladni napln sms bez priloh
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert resp['status'] == 400
    print(resp)
    result_error = resp.get('errors', {})
    assert result_error == expected_error


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_caseId])
def test_caseid_sms(ns_url, auth_params, auth, json_data, modified_json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    json_data['text'] = json_data['text'] + " " + url_name

    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=modified_json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_documentHash])
def test_documentHash_sms(ns_url, auth_params, auth, json_data, modified_json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    json_data['text'] = json_data['text'] + " " + url_name

    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=modified_json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# připrava na MSSQL dotazy
@pytest.mark.skip("pouze pro test connection")
@pytest.mark.parametrize("expected_result", [
    (True)
])
def test_db_connection(db_url, db_connection, expected_result):
    cursor = db_connection.cursor()
    notification_id = 'c0fdfabf-f859-4023-834b-e0004070768d'
    try:
        cursor.execute("""
                SELECT * 
                FROM AuditEvent
                WHERE AuditEventTypeId IN (?, ?) 
                AND JSON_VALUE(Detail, '$.body.objectsBefore.notificationId') = ?
                ORDER BY [TimeStamp] DESC
                """, ('AU_NOBY_013', 'AU_NOBY_012', notification_id)
                       )
        results = cursor.fetchall()
        found_records = bool(results)
        # Výpis výsledků.
        print(f"Results from {db_url['db_name']}:")
        for row in results:
            print(row)
    except pyodbc.Error as e:
        pytest.fail(f"Failed to execute query: {e}")

    assert found_records == expected_result
