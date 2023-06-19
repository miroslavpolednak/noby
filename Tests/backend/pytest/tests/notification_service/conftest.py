import datetime
import pyodbc

import datetime as datetime
import pytest
import requests
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

# pylint: disable=unused-import

URLS = {
    "dev_url": "https://ds-notification-dev.vsskb.cz:30016",
    "sit_url": "https://ds-notification-sit1.vsskb.cz:32016",
    "fat_url": "https://ds-notification-sit1.vsskb.cz:31016",
    "uat_url": "https://ds-notification-uat.vsskb.cz:33016",
}


def pytest_addoption(parser):
    parser.addoption("--ns-url", action="store", default="dev_url",
                     help="ns url")


@pytest.fixture(scope="session")
def ns_url(request):
    """Fixture pro nastavení URL adresy."""
    url_name = request.config.getoption("--ns-url")
    url = URLS[url_name]
    return {"url": url, "url_name": url_name}


@pytest.fixture(scope="function")
def auth(request, auth_params):
    """Fixture pro ověření uživatele.
    Parametry:
    auth_params: Parametry pro ověření uživatele.
    Výstup:
    tuple: Tuple obsahující uživatelské jméno a heslo.
    """

    username = request.param
    password = get_password(auth_params, username)
    return (username, password)


@pytest.fixture(scope="session")
def auth_params():
    """Fixture pro ověřovací parametry.
    Výstup:
    dict: Slovník obsahující ověřovací parametry."""

    return {
        "XX_INSG_RMT_USR_TEST": "dyasrykgSDSGNFN!!2dvcxgrttrrthyy",
        "XX_EPSY_RMT_USR_TEST": "5516...!!!sddsrgrgUTHTEREWQWeqee",
        "XX_SB_RMT_USR_TEST": "epdhqao965425800%!!%HRSOYIUREWQW",
        "XX_NOBY_RMT_USR_TEST": "ppmlesnrTWYSDYGDR!98538535634544"
    }


def get_password(auth_params, username):
    """Funkce pro získání hesla pro ověření uživatele.
        Parametry:
        auth_params: Slovník s ověřovacími parametry.
        username: Uživatelské jméno.
        Výstup:
        str: Heslo pro uživatelské jméno.
        """

    return auth_params[username]


@pytest.fixture(scope="module")
def authenticated_seqlog_session():
    session = requests.Session()

    response = session.post(
        url="http://172.30.35.51:6341/api/users/login",
        json={
             "Username": "seqadmin",
            "Password": "Rud514",
            "NewPassword": "",
        },
        verify=False,  # Přeskočit ověření certifikátu
    )

    if response.status_code != 200:
        raise ValueError("Přihlášení se nezdařilo")

    return session


@pytest.fixture()
def tomorrow_datetime():
    tomorrow = (datetime.date.today() + datetime.timedelta(days=1))
    return tomorrow.strftime("%Y-%m-%dT%H:%M:%S")


def get_current_date():
    today = (datetime.date.today())
    reformat_today = today.strftime("%Y-%m-%dT%H:%M:%S")
    return reformat_today


def greater_than_zero(items_count):
    return items_count > 0