import os
import sqlite3
import time

import pytest
import requests
import urllib3
from sqlalchemy.dialects.mssql.information_schema import columns
from sqlalchemy.orm import query

from Tests.backend.pytest.tests.notification_service.conftest import URLS
from ..json.request.mail_mpss_json import json_req_mail_mpss_full_attachments

collected_notification_ids = []
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)


@pytest.fixture(scope="session", autouse=True)
def collect_notification_ids():
    global collected_notification_ids
    yield
    # Kontrola existence souboru a jeho smazání, pokud existuje
    if os.path.exists('test_results.db'):
        os.remove('test_results.db')
    # Ukládání do SQLite databáze po dokončení všech testů
    try:
        conn = sqlite3.connect('test_results.db')
        c = conn.cursor()
        c.execute('''CREATE TABLE IF NOT EXISTS notification_ids (id TEXT, state INTEGER)''')
        c.executemany("INSERT INTO notification_ids VALUES (?, ?)", (collected_notification_ids))
        conn.commit()
    except Exception as e:
        print("Chyba při práci s databází:", e)
    finally:
        conn.close()


# postupně - je to tak 2 cally na 1 vteřinu, tak nžíe v range dám počet opakování
@pytest.mark.parametrize("run_number", range(10))  # Spustí test 1x opakovaně
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
@pytest.mark.parametrize("url_name, json_data", [
    ("fat_url", json_req_mail_mpss_full_attachments)
])
def test_mail_for_perf(run_number, url_name, auth_params, auth, json_data, mssql_connection):
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
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]

    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'

    cursor = mssql_connection.cursor()
    ###########################kotrola v SendEmail uložení Payloadu
    table_name = 'SendEmail'
    schema_name = 'DDS'  # Název schématu
    cursor.execute(f"""
            SELECT DocumentDataEntityId, Data, CreatedTime
            FROM {schema_name}.{table_name}
            WHERE DocumentDataEntityId = ?;
            """, (notification_id,)
                   )
    columns = cursor.fetchall()
    # Assert, že byl nalezen alespoň jeden záznam
    assert len(columns) > 0, "Žádný záznam nebyl nalezen"
    # Assert, že sloupec Data má hodnotu
    # Předpokládá se, že 'Data' je druhý sloupec v dotazu
    assert columns[0][1] is not None and columns[0][1] != "", "Sloupec Data je prázdný nebo None"
    # Pro ilustraci vypíše informace o sloupcích
    for col in columns:
        print(
            f"NSid: {col.DocumentDataEntityId}")

        ######################## update do db na state 2 (Unsent
        # Definice názvu tabulky a schématu
        table_name = 'EmailResult'
        schema_name = 'dbo'
        # Parametrizovaný SQL příkaz pro aktualizaci
        cursor.execute(f"""
            UPDATE {schema_name}.{table_name}
            SET State = 2
            WHERE Id = ?;
        """, (notification_id,))

        # Potvrzení změn v databázi
        mssql_connection.commit()

        ##Ověření provedení změn pomocí SELECT dotazu
        select_query = f"""
            SELECT Id, State, Resend, SenderType, Channel, CustomId FROM {schema_name}.{table_name}
            WHERE Id = ?;
        """
        cursor.execute(select_query, (notification_id,))
        row = cursor.fetchone()
        print(row)

        assert row[1] == 2, f"Očekávaný State je 2 UNSENT, ale získaný State je {row[1]}"

        # Uložení notificationId do globálního seznamu
        current_timestamp = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime())
        global collected_notification_ids
        collected_notification_ids.append((notification_id, row[1]))

        # Kontrola, že PAyload zustal v db
        table_name = 'SendEmail'
        schema_name = 'DDS'  # Název schématu
        cursor.execute(f"""
                    SELECT DocumentDataEntityId, Data, CreatedTime
                    FROM {schema_name}.{table_name}
                    WHERE DocumentDataEntityId = ?;
                    """, (notification_id,)
                       )
        columns = cursor.fetchall()
        # Assert, že byl nalezen alespoň jeden záznam
        assert len(columns) > 0, "Žádný záznam nebyl nalezen"
        # Assert, že sloupec Data má hodnotu
        # Předpokládá se, že 'Data' je druhý sloupec v dotazu
        assert columns[0][1] is not None and columns[0][1] != "", "Sloupec Data je prázdný nebo None"
        # Pro ilustraci vypíše informace o sloupcích
        for col in columns:
            print(
                f"NSid: {col.DocumentDataEntityId}")


# základní test
# @pytest.mark.skip(reason="pro ruční spouštění")
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("mssql_connection", [{"server": "fat", "database": "ns"}], indirect=True)
def test_update_state_for_perf(url_name, auth_params, auth, json_data, mssql_connection):
    """Test, kdy máme prerektivitu vytvořenou db s x záznamy id v sqlite.
        proběhne nacteni dat z test_results sqlite databaze a všem se do mssql aktualizuje stav"""
    # Připojení k SQLite databázi
    conn = sqlite3.connect('test_results.db')
    cursor = conn.cursor()

    # Načtení všech notification_id
    cursor.execute("SELECT id FROM notification_ids")
    notification_ids = [row[0] for row in cursor.fetchall()]

    # Zavření připojení
    conn.close()

    cursor = mssql_connection.cursor()
    ######################## update do db na state 2 (Unsent
    # Definice názvu tabulky a schématu
    table_name = 'EmailResult'
    schema_name = 'dbo'
    # Parametrizovaný SQL příkaz pro aktualizaci
    for notification_id in notification_ids:
        cursor.execute(f"""
            UPDATE {schema_name}.{table_name}
            SET State = 1
            WHERE Id = ?;
        """, (notification_id,))

    # Potvrzení změn a zavření databáze
    mssql_connection.commit()
    mssql_connection.close()
