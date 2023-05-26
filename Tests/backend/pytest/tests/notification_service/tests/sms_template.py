import pytest
import requests

from ..conftest import URLS
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb, json_req_sms_sb, json_req_sms_basic_alex
from ..json.request.sms_template_json import json_req_sms_full_template, json_req_sms_template_bad_basic_without_identifier, \
    json_req_sms_template_bad_basic_without_identifier_scheme, \
    json_req_sms_template_bad_basic_without_identifier_identity


@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_sms_full_template])
def test_sms_template(ns_url,  auth_params, auth, json_data):
    """SMS s template z tabulky CodebookService.dbo.SmsNotificationType"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
@pytest.mark.parametrize("json_data, expected_error", [
    (json_req_sms_template_bad_basic_without_identifier, {
            'Identifier.Identity': ['The Identity field is required.'],
            'Identifier.IdentityScheme': ['The IdentityScheme field is required.']
        }),
    (json_req_sms_template_bad_basic_without_identifier_scheme, {
            'Identifier.IdentityScheme': ['The IdentityScheme field is required.']
        }),
    (json_req_sms_template_bad_basic_without_identifier_identity, {
            'Identifier.Identity': ['The Identity field is required.']
        }),
])
def test_sms_template_bad_identifier_template(ns_url, auth_params, auth, json_data, expected_error):
    """SMS s template z tabulky CodebookService.dbo.SmsNotificationType"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
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
    assert resp['status'] == 400
    print(resp)
    result_error = resp.get('errors', {})
    assert result_error == expected_error
