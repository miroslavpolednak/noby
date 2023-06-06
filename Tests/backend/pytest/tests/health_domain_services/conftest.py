import datetime
import pyodbc

import datetime as datetime
import pytest
import requests
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

# pylint: disable=unused-import

URLS = {
    "uat_url": "https://ds-discovery-uat.vsskb.cz:33000",
}


def pytest_addoption(parser):
    parser.addoption("--discovery-url", action="store", default="dev_url",
                     help="discovery url")


@pytest.fixture(scope="session")
def ns_url(request):
    """Fixture pro nastavení URL adresy."""
    url_name = request.config.getoption("--discovery-url")
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


#zatim není user potrebny
@pytest.fixture(scope="session")
def auth_params():
    """Fixture pro ověřovací parametry.
    Výstup:
    dict: Slovník obsahující ověřovací parametry."""

    return {
        "username": "psw"
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


def get_noby_cookies():
    session = requests.session()
    session.post(
        discovery + "/users/signin",
        json={
            "Login": "99917587"
        }
    )
    return session.cookies