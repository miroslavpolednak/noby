import pytest
import requests

from ..conftest import URLS
from ..json.request.sms import json_req_sms_basic, json_req_sms_basic_full, json_req_sms_basic_epsy, \
    json_req_sms_basic_insg
from ..json.request.sms_template import json_req_sms_full_template, json_req_sms_basic_template


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic, json_req_sms_basic_full])
def test_sms(url_name,  auth_params, auth, json_data):
    """Parametry:
    ns_url: URL adresa pro testování.
    auth_params: Slovník s ověřovacími parametry.
    auth: Tuple obsahující uživatelské jméno a heslo.
    json_data: JSON data pro odesílání SMS.
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


@pytest.mark.parametrize("url_name", ["uat_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_full_template, json_req_sms_basic_template])
def test_sms_template(url_name,  auth_params, auth, json_data):
    """SMS s template z tabulky CodebookService.dbo.SmsNotificationType"""

    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/smsFromTemplate",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    assert resp["notificationId"] != ""


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic_insg, json_req_sms_basic_epsy])
def test_sms_combination_security(url_name,  auth_params, auth, json_data):
    """test pto kombinaci všech uživatelů s basic sms"""

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
    # chybi dodelat asserty do databaze


#pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth, json_data",
                         [
                            ("XX_INSG_RMT_USR_TEST", json_req_sms_basic_insg),
                            ("XX_EPSY_RMT_USR_TEST", json_req_sms_basic_epsy)

], indirect=["auth"])
def test_sms_basic_security(auth_params, auth, json_data, url_name):
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
