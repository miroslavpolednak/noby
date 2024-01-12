import uuid
import datetime
from time import sleep
from urllib.parse import urlencode, quote

from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal

"""
jak provolat s upravou additional argument, ze složky: notification_service> pytest .\tests\sms.py --ns-url fat_url --db-url fat_db
"""
import pyodbc
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

import pytest
import requests

from ..conftest import URLS, greater_than_zero, ns_url, auth, get_today_date_strings
from ..json.request.seg_log import json_req_basic_log
from ..json.request.sms_json import json_req_sms_basic_insg, json_req_sms_basic_full, json_req_sms_basic_epsy_kb, \
    json_req_sms_basic_insg, json_req_sms_bez_logovani_kb_sb, json_req_sms_logovani_kb_sb_E2E, json_req_sms_sb, \
    json_req_sms_bad_basic_without_identifier, json_req_sms_bad_basic_without_identifier_scheme, \
    json_req_sms_bad_basic_without_identifier_identity, json_req_sms_basic_insg_uat, json_req_sms_mpss_archivator, \
    json_req_sms_kb_archivator, json_req_sms_basic_insg_fat, json_req_sms_basic_insg_sit, json_req_sms_basic_insg_e2e, \
    json_req_sms_caseId, json_req_sms_documentHash, json_req_sms_basic_kb_insg, json_req_sms_logovani_mpss_sb, \
    json_req_sms_bez_logovani_mpss_sb, json_req_sms_logovani_kb_insg
from ..json.request.sms_template_json import json_req_sms_full_template


# test pro additional parameters napr. --ns-url sit_url

@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [(json_req_sms_basic_insg_e2e)])
def test_sms_result(ns_url, auth_params, auth, json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]

    # získání aktuální statistiky
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    time_from, time_to = get_today_date_strings()
    # Přidání datumů do URL
    statistics_url = f"{URLS[url_name]}/v1/notification/result/statistics?timeFrom={time_from}&timeTo={time_to}"

    session = requests.session()
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics = resp.json()['statistics']
    print(statistics)

    # odeslání sms
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 200

    # aktuální statistika
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics_actual = resp.json()['statistics']
    print(statistics_actual)
    assert statistics_actual['sms']['inProgress'] == statistics['sms']['inProgress'] + 1

    sleep(15)
    # aktuální statistika
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics_sent = resp.json()['statistics']
    print(statistics_sent)
    assert statistics_sent['sms']['inProgress'] == statistics['sms']['inProgress']
    assert statistics_sent['sms']['delivered'] == statistics['sms']['delivered'] + 1


@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [(json_req_mail_mpss_basic_legal)])
def test_email_result(ns_url, auth_params, auth, json_data):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]

    # získání aktuální statistiky
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    time_from, time_to = get_today_date_strings()
    # Přidání datumů do URL
    statistics_url = f"{URLS[url_name]}/v1/notification/result/statistics?timeFrom={time_from}&timeTo={time_to}"

    session = requests.session()
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics = resp.json()['statistics']
    print(statistics)

    # odeslání mailu
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 200

    # aktuální statistika
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics_actual = resp.json()['statistics']
    print(statistics_actual)
    assert statistics_actual['email']['inProgress'] == statistics['email']['inProgress'] + 1




@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [(json_req_sms_basic_insg)])
def test_sms_detail_result(ns_url, auth_params, auth, json_data):
    """
    Test je prováděn pouze na SMS, protože máme rychlejší ověření doručení sms, než mailu, to trvá až 61 vteřin do stavu SENT.
    """
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]

    # získání aktuální statistiky
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]
    time_from, time_to = get_today_date_strings()
    # Přidání datumů do URL
    statistics_url = f"{URLS[url_name]}/v1/notification/result/detailed-statistics?statisticsDate={time_from}"

    session = requests.session()
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics = resp.json()['statistics']
    detail_statistics = resp.json()['results']
    first_item = detail_statistics[0]
    assert all(key in first_item for key in
               ['notificationId', 'state', 'channel', 'requestTimestamp'])  # Kontrola přítomnosti klíčů
    assert all(first_item[key] for key in
               ['notificationId', 'state', 'channel', 'requestTimestamp'])  # Kontrola, že klíče mají hodnoty

    # odeslání sms
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/sms",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 200
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""

    # aktuální statistika
    resp = session.get(
        statistics_url,
        auth=(noby_username, noby_password),
        verify=False
    )
    statistics_actual = resp.json()['statistics']
    assert statistics_actual['sms']['inProgress'] == statistics['sms']['inProgress'] + 1

    detail_statistics_actual = resp.json()['results']

    # Ověření, že detail_statistics_actual obsahuje notification_id
    notification_id_in_actual = any(item['notificationId'] == notification_id for item in detail_statistics_actual)

    # Ověření, že detail_statistics neobsahuje notification_id
    # Předpokládá se, že detail_statistics je již definován
    notification_id_in_detail = any(item['notificationId'] == notification_id for item in detail_statistics)
    assert notification_id_in_actual and not notification_id_in_detail