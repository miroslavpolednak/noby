import pytest
import requests
from ..json.request.sms import json_req_sms_basic, json_req_sms_basic_full, json_req_sms_basic_epsy, \
    json_req_sms_basic_insg


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_basic, json_req_sms_basic_full])
def test_sms_basic(ns_url, auth_params, auth, json_data):
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
        ns_url["url"] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    # chybi dodelat asserty do databaze


#pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("auth, json_data",
                         [
                            ("XX_EPSY_RMT_USR_TEST", json_req_sms_basic_epsy),
                            ("XX_INSG_RMT_USR_TEST", json_req_sms_basic_insg)

], indirect=["auth"])
def test_sms_basic_security(ns_url, auth_params, auth, json_data):
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        ns_url["url"] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert resp["status"] == 200
