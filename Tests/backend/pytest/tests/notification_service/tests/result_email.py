import time
import uuid
import urllib3

from ..json.request.mail_kb_json import json_req_mail_kb_max_attachments
from ..json.request.mail_mpss_json import json_req_mail_mpss_max_attachments

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
import pytest
import requests

from ..conftest import URLS
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb_E2E, \
    json_req_sms_basic_full_for_search
from ..json.request.sms_template_json import json_req_sms_full_template


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_max_attachments,
                                       json_req_mail_kb_max_attachments])
def test_get_email_notification_id(auth_params, auth, json_data, ns_url):

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

    #vola GET opet, abz si overil doruceni
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