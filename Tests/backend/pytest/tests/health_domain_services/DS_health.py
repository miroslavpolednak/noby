import pytest
import requests

from Tests.backend.pytest.tests.health_domain_services.conftest import URLS


#NOBY vraci okej, ale v databázi padnou kombinace, ktere nemaji pro sebe MCS kod
#@pytest.mark.parametrize("url_name", ["uat_url"])
def test_ds_health_check():
    """test pro kombinaci všech uživatelů s basic sms"""
    session = requests.session()
    resp = session.get(
        "https://ds-discovery-uat.vsskb.cz:33000/health",
        verify=False
    )
    resp = resp.json()
    resp = resp.json()