import pytest
import requests

from ..conftest import URLS
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, \
    json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural, json_req_mail_mpss_bad_natural_legal, \
    json_req_mail_mpss_max_attachments, json_req_mail_mpss_bad_11_attachments, json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_mpss_basic, json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_scheme_mpss_basic


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, json_req_mail_mpss_full_attachments,
                                       json_req_mail_mpss_full_natural, ])
def test_mail(url_name,  auth_params, auth, json_data):
    """kladny test"""

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


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_max_attachments])
def test_mail_max_attachments(url_name,  auth_params, auth, json_data):
    """max priloh klady test"""

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


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_11_attachments])
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


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_NOBY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal])
def test_mail_negative(url_name,  auth_params, auth, json_data):
    """negativní test"""

    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 400


#pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth, json_data",
                         [
                            ("XX_INSG_RMT_USR_TEST", json_req_mail_mpss_bad_natural_legal, json_req_mail_bad_identifier_identity_mpss_basic)
], indirect=["auth"])
def test_mail_bad_request(auth_params, auth, json_data, url_name):
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
    assert resp['status'] == 400
    print(resp)
    errors = resp.get('errors', {})
    assert any('FirstName required' in error for error_list in errors.values() for error in error_list)
    assert any('Surname required' in error for error_list in errors.values() for error in error_list)
    assert any('Name required' in error for error_list in errors.values() for error in error_list)


#TODO: dodelat assrty pro errors
#pro testy bad requesty
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth, json_data",
                         [
                            ("XX_INSG_RMT_USR_TEST", json_req_mail_bad_identifier_mpss_basic, json_req_mail_bad_identifier_scheme_mpss_basic, json_req_mail_bad_identifier_identity_mpss_basic)
], indirect=["auth"])
def test_mail_bad_request(auth_params, auth, json_data, url_name):
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
    assert resp['status'] == 400
    print(resp)
    errors = resp.get('errors', {})
