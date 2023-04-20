import uuid
from time import sleep
from urllib.parse import urlencode, quote

import pytest
import requests

from ..conftest import URLS, greater_than_zero
from ..json.request.sms_json import json_req_sms_basic, json_req_sms_basic_full, json_req_sms_basic_epsy, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani, json_req_sms_logovani, json_req_sms_sb, json_req_sms_basic_alex, \
    json_req_sms_bad_basic_without_identifier, json_req_sms_bad_basic_without_identifier_scheme, \
    json_req_sms_bad_basic_without_identifier_identity, json_req_sms_basic_uat
from ..json.request.sms_template import json_req_sms_full_template, json_req_sms_basic_template, \
    json_req_sms_full_template_uat, json_req_sms_basic_template_uat


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("dev_url", json_req_sms_basic),
    ("uat_url", json_req_sms_basic_uat)])
def test_sms(url_name,  auth_params, auth, json_data):
    """
    uvodni test pro zakladni napln sms bez priloh
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


@pytest.mark.skip
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_sb])
def test_sms_sb(url_name,  auth_params, auth, json_data):
    """
    SB test do budoucna, který ale ještě nemá svůj mcs type sms
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


#TODO: dořešit assert. je to dle https://wiki.kb.cz/pages/viewpage.action?pageId=507386569 Text auditního logu - to nesmí být v events
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("custom_id, json_data, expected_results", [
    ("loguji", json_req_sms_logovani, greater_than_zero),
    ("neloguji", json_req_sms_bez_logovani, 0)
])
def test_sms_log(url_name,  auth_params, auth, custom_id, json_data, expected_results, authenticated_seqlog_session):
    """test logovani - zalogujeme, nezalogujeme do seq"""

    username = auth[0]
    password = auth[1]
    #vytvoření unikátního ID pro customId pro dohledani logu
    uuid_str = uuid.uuid4().hex
    unique_custom_id = f"{uuid_str[:4]}{custom_id}{uuid_str[4:8]}"
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
    sleep(2)

    #Kontrola v seq logu
    params = {"filter": f"@Message like '%{unique_custom_id}%' ci or @Exception like '%{unique_custom_id}%' ci or @Message like '%{notification_id}%' ci or @Exception like '%{notification_id}%' ci", "shortCircuitAfter": 100}

    encoded_params = "&".join(f"{key}={quote(str(value), safe='')}" for key, value in params.items())
    url = f"http://172.30.35.51:6341/api/events/signal?{encoded_params}"

    resp = authenticated_seqlog_session.post(
        url,
        json={"Title": "New Signal", "Description": None, "Filters": [], "Columns": [], "IsWatched": False, "Id": None,
              "Grouping": "Inferred", "ExplicitGroupName": None, "OwnerId": "user-admin",
              "Links": {"Create": "api/signals/"}})
    print(resp.json())

    events = resp.json()['Events']
    if callable(expected_results):
        assert expected_results(
            len(events)), f"Očekávaná podmínka nebyla splněna: počet položek je {len(events)}, ale měl být větší než 0"
    else:
        assert len(
            events) == expected_results, f"Očekávaný počet položek se neshoduje: {len(events)} != {expected_results}"


@pytest.mark.parametrize("url_name", ["dev_url", "uat_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg, json_req_sms_basic_epsy])
def test_sms_combination_security(url_name,  auth_params, auth, json_data):
    """test pro kombinaci všech uživatelů s basic sms"""

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


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth, json_data",
                         [
                             ("XX_INSG_RMT_USR_TEST", json_req_sms_basic_insg),
                             ("XX_EPSY_RMT_USR_TEST", json_req_sms_basic_epsy)
                         ], indirect=["auth"])
def test_sms_basic_security(auth_params, auth, json_data, url_name):
    """
   Testuje zabezpečení: kladné testy pro usery a jejich msc type kody.
   Použita vnorena parametrizace pro různé uživatele a type sms.
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
    assert resp["notificationId"] != ""


@pytest.mark.skip
@pytest.mark.parametrize("url_name", ["uat_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_alex])
def test_sms_basic_alex(url_name,  auth_params, auth, json_data):
    """SMS s template z tabulky CodebookService.dbo.SmsNotificationType"""

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


#TODO: assert pro error chybu na chybějící parametry
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_bad_basic_without_identifier, json_req_sms_bad_basic_without_identifier_scheme, json_req_sms_bad_basic_without_identifier_identity ])
def test_sms_bad_requests(url_name,  auth_params, auth, json_data):
    """uvodni test pro zakladni napln sms bez priloh
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
    assert resp['status'] == 400
    print(resp)
    errors = resp.get('errors', {})