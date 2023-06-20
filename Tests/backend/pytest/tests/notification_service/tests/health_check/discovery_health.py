import uuid
from time import sleep
from urllib.parse import urlencode, quote
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

import pytest
import requests

from ...conftest import URLS


@pytest.mark.parametrize("url_name", [
    "dev_url_discovery", "fat_url_discovery", "sit_url_discovery", "uat_url_discovery"])
def test_discovery_health(url_name):
    """
    health check SS sluzeb - assert zatim jen na notification service
    """

    session = requests.session()
    resp = session.get(
        URLS[url_name] + "/health",
        verify=False
    )
    resp = resp.json()
    notification_service = resp['results']['CIS:NotificationService']
    assert notification_service['status'] == 'Healthy'

    print(resp)

