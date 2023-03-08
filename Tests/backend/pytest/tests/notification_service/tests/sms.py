import pytest
import requests
from request.sms import json_req_sms_basic


@pytest.mark.parametrize("json_data", [json_req_sms_basic])
def test_sms_basic(ns_url, json_data):
    session = requests.session()
    resp = session.post(
        ns_url["url"] + "/v1/notification/sms",
        json=json_data,
        auth=ns_url["auth"],
        verify=False
    )
    resp = resp.json()
    print(resp)
