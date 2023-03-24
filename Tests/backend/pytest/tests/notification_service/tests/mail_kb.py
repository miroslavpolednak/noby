import pytest
import requests

from ..conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_bad_11_attachments


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_kb_bad_11_attachments])
def test_mail_11_attachments(url_name,  auth_params, auth, json_data):
    """max priloh klady test, ale MCS zařízne"""

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