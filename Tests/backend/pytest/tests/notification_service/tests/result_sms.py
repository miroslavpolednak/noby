import time
import uuid
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
import pytest
import requests

from ..conftest import URLS
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb_E2E, \
    json_req_sms_basic_full_for_search
from ..json.request.sms_template_json import json_req_sms_full_template


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg])
def test_get_sms_notification_id_states(auth_params, auth, json_data, ns_url):
    """uvodni test pro zakladni napln sms bez priloh, realne sms neprijde - fake cislo
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
    assert resp['channel'] == 'Sms'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    expected_sms_data = json_req_sms_basic_insg.copy()

    # Odebere processingPriority, customId, documentid, text z expected_sms_data - insg nechce tento atribut vracet
    for attr in ["processingPriority", "text"]:
        if attr in expected_sms_data:
            del expected_sms_data[attr]

    # Převede phoneNumber na objekt s countryCode a nationalNumber
    phone_number = expected_sms_data.pop("phoneNumber")
    expected_sms_data["phone"] = {
        "countryCode": phone_number[:4],
        "nationalNumber": phone_number[4:]}
    assert resp["requestData"]["smsData"] == expected_sms_data
    time.sleep(15)

    #vola GET opet, abz si overil doruceni
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
    assert resp['state'] == 'Unsent'
    errors = resp.get('errors', [])
    assert len(errors) > 0, 'No errors present'
    assert errors[0]['code'] == 'SMS-MCH-04'
    assert errors[0]['message'] == 'Failed connection to the operator - message cannot be sent'

#TODO: koukni na response GET search, ve swagger vraci i vyparsovane parametry
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_full_for_search])
def test_get_sms_notification_search(ns_url,  auth_params, auth, json_data):
    """test pro vygenerovani sms a jeji nasledne vyhledani
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    unique_custom_id = f"{uuid.uuid4()}"
    json_req_sms_basic_full_for_search["customId"] = unique_custom_id
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

    #volani search
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    session = requests.session()
    resp = session.get(
        URLS[url_name] + "/v1/notification/result/search",
        params={
            "identity": json_req_sms_basic_full_for_search["identifier"]["identity"],
            "identityScheme": json_req_sms_basic_full_for_search["identifier"]["identityScheme"],
            "customId": unique_custom_id,
            "documentId": json_req_sms_basic_full_for_search["documentId"]
        },
        auth=(noby_username, noby_password),
        verify=False
    )

    resp = resp.json()[0]
    assert resp['notificationId'] == notification_id
    valid_states = ['Sent', 'InProgress']
    assert resp['state'] in valid_states, f"Invalid state: {resp['state']}, expected one of {valid_states}"
    assert resp['channel'] == 'Sms'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    expected_sms_data = json_req_sms_basic_full_for_search.copy()

    # Odebere processingPriority, customId, documentid, text z expected_sms_data - insg nechce tento atribut vracet
    for attr in ["processingPriority", "customId", "documentId", "identifier", "text", "caseId"]:
        if attr in expected_sms_data:
            del expected_sms_data[attr]

    # Převede phoneNumber na objekt s countryCode a nationalNumber
    phone_number = expected_sms_data.pop("phoneNumber")
    expected_sms_data["phone"] = {
        "countryCode": phone_number[:4],
        "nationalNumber": phone_number[4:]}
    assert resp["requestData"]["smsData"] == expected_sms_data


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
def test_get_sms_notification_id_vulnerability(auth_params, auth, ns_url):
    """test zranitelnosti proti skriptování
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/<test_validace>",
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    error_message = notification['errors']['id'][0]
    assert error_message == 'The value \'%3Ctest_validace%3E\' is not valid.'

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
def test_get_sms_notification_search_vulnerability(ns_url,  auth_params, auth):
    """test zranitelnosti proti skriptování
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.get(
        URLS[url_name] + "/v1/notification/result/search",
        params={
            "identity": "<identity>",
            "identityScheme": "<identityScheme>",
            "customId": "<customId>",
            "documentId": "<documentId>"
        },
        auth=(username, password),
        verify=False
    )
    expected_error = {'302': ['Invalid Identity.'],
                      '304': ['Invalid IdentityScheme.'],
                      '306': ['Invalid CustomId.'],
                      '307': ['Invalid DocumentId.']}
    error = resp.json()['errors']
    assert error == expected_error, f'Expected {expected_error}, but got {error}'

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'



@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_full_for_search])
def test_get_sms_notification_search_caseId(ns_url,  auth_params, auth, json_data):
    """test pro vygenerovani sms a jeji nasledne vyhledani
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    unique_custom_id = f"{uuid.uuid4()}"
    json_req_sms_basic_full_for_search["customId"] = unique_custom_id
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

    #volani search
    # Pro druhý GET request potřebujete přihlašovací údaje NOBY uživatele
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    session = requests.session()
    resp = session.get(
        URLS[url_name] + "/v1/notification/result/search",
        params={
            "customId": unique_custom_id,
            "caseId": json_req_sms_basic_full_for_search["caseId"]
        },
        auth=(noby_username, noby_password),
        verify=False
    )
    assert resp.status_code == 200
    assert str(notification_id) in resp.text
    assert str('"caseId":303062934') in resp.text


@pytest.mark.parametrize("auth", ["XX_KBINSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_full_for_search])
def test_get_sms_notification_search_bad_auth(ns_url,  auth_params, auth, json_data):
    """test pro vygenerovani sms a jeji nasledne vyhledani
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    unique_custom_id = f"{uuid.uuid4()}"
    json_req_sms_basic_full_for_search["customId"] = unique_custom_id
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

    #volani search
    session = requests.session()
    resp = session.get(
        URLS[url_name] + "/v1/notification/result/search",
        params={
            "identity": json_req_sms_basic_full_for_search["identifier"]["identity"],
            "identityScheme": json_req_sms_basic_full_for_search["identifier"]["identityScheme"],
            "customId": unique_custom_id,
            "documentId": json_req_sms_basic_full_for_search["documentId"]
        },
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 403

    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(username, password),
        verify=False
    )

    assert resp.status_code == 403