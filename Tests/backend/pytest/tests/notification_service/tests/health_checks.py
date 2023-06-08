
import pytest
import requests
import time
import uuid
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
from ..conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_basic_legal
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full_for_search, \
    json_req_sms_basic_insg_e2e
from ..json.request.sms_template_json import json_req_sms_full_template, json_req_real_sms_full_template


#est pro additional parameters napr. --ns-url sit_url
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg_e2e])
def test_get_sms_notification_id_states(ns_url, auth_params, auth, json_data, modified_json_data):
    """uvodni test pro zakladni napln sms bez priloh
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
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

    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    valid_states = ['Sent', 'InProgress']
    assert resp['state'] in valid_states, f"Invalid state: {resp['state']}, expected one of {valid_states}"
    assert resp['channel'] == 'Sms'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    expected_sms_data = json_req_sms_basic_insg_e2e.copy()

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
    time.sleep(10)

    #vola GET opet, abz si overil doruceni
    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    assert resp['state'] == 'Delivered'
    assert resp['state'] == 'Delivered'


#TODO: koukni na response GET search, ve swagger vraci i vyparsovane parametry
#est pro additional parameters napr. --ns-url sit_url
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

    resp = resp.json()[0]
    assert resp['notificationId'] == notification_id
    valid_states = ['Sent', 'InProgress']
    assert resp['state'] in valid_states, f"Invalid state: {resp['state']}, expected one of {valid_states}"
    assert resp['channel'] == 'Sms'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    expected_sms_data = json_req_sms_basic_full_for_search.copy()

    # Odebere processingPriority, customId, documentid, text z expected_sms_data - insg nechce tento atribut vracet
    for attr in ["processingPriority", "customId", "documentId", "identifier", "text"]:
        if attr in expected_sms_data:
            del expected_sms_data[attr]

    # Převede phoneNumber na objekt s countryCode a nationalNumber
    phone_number = expected_sms_data.pop("phoneNumber")
    expected_sms_data["phone"] = {
        "countryCode": phone_number[:4],
        "nationalNumber": phone_number[4:]}
    assert resp["requestData"]["smsData"] == expected_sms_data


#základní test pro mail
#est pro additional parameters napr. --ns-url sit_url
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal, json_req_mail_kb_basic_legal])
def test_mail_health_check(ns_url,  auth_params, auth, json_data):
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


#zakladni test pro template
#test pro additional parameters napr. --ns-url sit_url
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_real_sms_full_template])
def test_sms_template(ns_url,  auth_params, auth, json_data, modified_template_json_data):
    """SMS s template z tabulky CodebookService.dbo.SmsNotificationType"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/smsFromTemplate",
        json=modified_template_json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    assert resp["notificationId"] != ""