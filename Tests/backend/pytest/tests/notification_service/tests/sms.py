import pytest
import requests
from request.sms import json_req_sms_basic, json_req_sms_basic_full
from ..conftest import auth_params


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
