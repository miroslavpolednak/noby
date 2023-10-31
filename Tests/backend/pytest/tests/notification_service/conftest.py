import copy
import datetime as datetime

import pyodbc
import pytest
import requests
from sqlalchemy.orm import declarative_base

Base = declarative_base()

# pylint: disable=unused-import

URLS = {
    "dev_url": "https://ds-notification-dev.vsskb.cz:30016",
    "fat_url": "https://ds-notification-fat.vsskb.cz:31016",
    "sit_url": "https://ds-notification-sit1.vsskb.cz:32016",
    "uat_url": "https://ds-notification-uat.vsskb.cz:33016",

    "dev_url_discovery": "https://ds-discovery-dev.vsskb.cz:30011",
    "fat_url_discovery": "https://ds-discovery-fat.vsskb.cz:31011",
    "sit_url_discovery": "https://ds-discovery-sit1.vsskb.cz:32011",
    "uat_url_discovery": "https://ds-discovery-uat.vsskb.cz:33011"
}

DB_SERVERS = {
    'dev_db': 'adpra173.vsskb.cz',
    'fat_db': r'adpra173.vsskb.cz\FAT',
    'sit_db': r'adpra173.vsskb.cz\SIT',
    'uat_db': r'adpra173.vsskb.cz\UAT1'
}

DB_TEMPLATE = {
    'database': 'NobyAudit',
    'user': 'testsql',
    'password': 'Rud514',
}


def pytest_addoption(parser):
    parser.addoption("--ns-url", action="store", default="dev_url",
                     help="ns url"),
    parser.addoption("--db-url", action="store", default="dev_db",
                     help="db url")


@pytest.fixture(scope="session")
def ns_url(request):
    """Fixture pro nastavení URL adresy."""
    url_name = request.config.getoption("--ns-url")
    url = URLS[url_name]
    return {"url": url, "url_name": url_name}


@pytest.fixture(scope="session")
def db_url(request):
    """Fixture pro nastavení URL adresy."""
    db_name = request.config.getoption("--db-url")
    db = DB_SERVERS[db_name]
    return {"db": db, "db_name": db_name}


#napojeni na MSSQL noby

@pytest.fixture(scope='function')
def db_connection(db_url):
    connection_string = (
        fr"Driver={{ODBC Driver 17 for SQL Server}};"
        fr"Server={db_url['db']};"
        fr"Database={DB_TEMPLATE['database']};"
        fr"UID={DB_TEMPLATE['user']};"
        fr"PWD={DB_TEMPLATE['password']};"
    )
    connection = pyodbc.connect(connection_string)
    yield connection
    connection.close()


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
        "XX_NOBY_RMT_USR_TEST": "ppmlesnrTWYSDYGDR!98538535634544",
        "XX_KBINSG_RMT_USR_TEST": "hgfDRYTHSDIOJFd!!344546789......"
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
        url="https://172.30.35.51:6341/api/users/login",
        json={
             "Username": "seqadmin",
            "Password": "Rud514",
            "NewPassword": "",
        },
        verify=False  # Přeskočit ověření certifikátu
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


@pytest.fixture
def modified_json_data(request, ns_url):
    json_data = request.node.get_closest_marker("parametrize").args[1][0]
    modified_data = copy.deepcopy(json_data)  # vytvoříme kopii, abychom nezměnili původní data
    # Přidáme aktuální datum a čas
    now = datetime.datetime.now()
    date_time = now.strftime("%Y-%m-%d %H:%M:%S")  # formátuje datum a čas
    # seskladani smsky
    modified_data['text'] = " Prostredi: " + ns_url["url_name"] + ", " + " Cas provedeni: " + date_time + ", Zprava: " + modified_data['text']
    return modified_data

@pytest.fixture
def modified_template_json_data(request, ns_url):
    json_data = request.node.get_closest_marker("parametrize").args[1][0]
    modified_data = copy.deepcopy(json_data)  # vytvoříme kopii, abychom nezměnili původní data

    # Přidáme aktuální datum a čas
    now = datetime.datetime.now()
    date_time = now.strftime("%Y-%m-%d %H:%M:%S")  # formátuje datum a čas

    # Seskladani smsky
    for placeholder in modified_data['placeholders']:
        if placeholder['key'] == 'zadej':
            placeholder['value'] = " Prostredi: " + ns_url["url_name"] + ", " + " Cas provedeni: " + date_time + ", Zprava: " + placeholder['value']

    return modified_data


@pytest.fixture
def modified_json_data_health(request, url_name):
    marker = [m for m in request.node.iter_markers(name="parametrize")]
    json_data = None
    for m in marker:
        if "json_data" in m.args[0]:
            json_data = m.args[1][0]
    modified_data = copy.deepcopy(json_data)  # vytvoříme kopii, abychom nezměnili původní data
    # Přidáme aktuální datum a čas
    now = datetime.datetime.now()
    date_time = now.strftime("%Y-%m-%d %H:%M:%S")  # formátuje datum a čas
    # seskladani smsky
    modified_data['text'] = " Prostredi: " + url_name + ", " + " Cas provedeni: " + date_time + ", Zprava: " + modified_data['text']
    return modified_data


@pytest.fixture
def modified_template_json_data_health(request, url_name):
    marker = [m for m in request.node.iter_markers(name="parametrize")]
    json_data = None
    for m in marker:
        if "json_data" in m.args[0]:
            json_data = m.args[1][0]

    modified_data = copy.deepcopy(json_data)  # vytvoříme kopii, abychom nezměnili původní data

    # Přidáme aktuální datum a čas
    now = datetime.datetime.now()
    date_time = now.strftime("%Y-%m-%d %H:%M:%S")  # formátuje datum a čas

    # Seskladani smsky
    for placeholder in modified_data['placeholders']:
        if placeholder['key'] == 'zadej':
            placeholder['value'] = " Prostredi: " + url_name + ", " + " Cas provedeni: " + date_time + ", Zprava: " + placeholder['value']

    return modified_data