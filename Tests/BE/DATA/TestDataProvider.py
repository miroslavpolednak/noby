from typing import List

import os
from common import Log, SqliteDbManager, find_folder_tests_be

class TestDataProvider():

    __log = Log.getLogger(__file__)
    __db_manager = SqliteDbManager(os.path.join(find_folder_tests_be(), 'DATA', 'db', 'DB.db'))

    @staticmethod
    def reset():
        path_to_sql: str = os.path.join(find_folder_tests_be(), 'DATA', 'db', 'sql_init.sql')
        TestDataProvider.__db_manager.exec_scripts([path_to_sql])

    @staticmethod
    def import_files(files, environments):
        #TODO:
        pass