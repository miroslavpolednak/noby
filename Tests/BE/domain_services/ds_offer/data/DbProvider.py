import os

from typing import List
from common import SqliteDbManager

# -----------------------------------------------------------------------------------------------------------------------
PATH_TO_FOLDER_DB: str = os.path.join(os.path.abspath(os.path.dirname(__file__)), 'db')
PATH_TO_FOLDER_DB_SCRIPTS: str = os.path.join(PATH_TO_FOLDER_DB, 'scripts')

PATH_TO_DB: str = os.path.join(PATH_TO_FOLDER_DB, 'DbOffer.db')
PATH_TO_SQL: str = os.path.join(PATH_TO_FOLDER_DB_SCRIPTS, '01_build_db.sql')
PATH_TO_SQL_SEED: str = os.path.join(PATH_TO_FOLDER_DB_SCRIPTS, '02_insert_simulation_data.sql')
# -----------------------------------------------------------------------------------------------------------------------

class DbProvider():
    def __init__(self):
        self._db_manager = SqliteDbManager(PATH_TO_DB)
        
    def __str__ (self):
        return f'DbProviderDbProvider [DbManager: {self._db_manager}]'

    def prepare_input_data(self):
        file_paths: List[str] = [
            PATH_TO_SQL,
            PATH_TO_SQL_SEED
        ]
        self._db_manager.exec_scripts(file_paths)

    def load(self):
        rows = self._db_manager.exec_query('SELECT * FROM MortgageSimulationInputs')
        for r in rows:
            print(r)

    def loadBasicParameters(self, pk: int):
        rows = self._db_manager.exec_query(f'SELECT * FROM BasicParameters WHERE BasicParametersPk = {pk}')
        #print(rows)
        return rows[0]
    