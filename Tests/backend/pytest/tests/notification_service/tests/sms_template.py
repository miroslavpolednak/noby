import pytest
import requests

from ..conftest import URLS
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb, json_req_sms_sb, json_req_sms_basic_alex
from ..json.request.sms_template import json_req_sms_full_template, json_req_sms_basic_template, \
    json_req_sms_full_template_uat, json_req_sms_basic_template_uat


@pytest.mark.parametrize("url_name", ["dev_url"])
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


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("dev_url", json_req_sms_full_template),
    ("dev_url", json_req_sms_basic_template)
])
def test_sms_templates_with_various_env(url_name,  auth_params, auth, json_data):
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
