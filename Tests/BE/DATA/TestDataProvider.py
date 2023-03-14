from typing import List

import os
from common import Log, SqliteDbManager, find_folder_tests_be, EEnvironment
from .ESource import ESource

class TestDataProvider():

    __log = Log.getLogger(__file__)
    __db_manager = SqliteDbManager(os.path.join(find_folder_tests_be(), 'DATA', 'db', 'DB.db'))
    __folder_tests_be: str = find_folder_tests_be()

    @staticmethod
    def reset():
        path_to_sql: str = os.path.join(TestDataProvider.__folder_tests_be, 'DATA', 'db', 'sql_init.sql')
        TestDataProvider.__db_manager.exec_scripts([path_to_sql])

    @staticmethod
    def import_custom(folder_path: List[str], environments: EEnvironment):

        # build path to folder
        path_to_folder: str = os.path.join(TestDataProvider.__folder_tests_be, 'DATA', 'custom', *folder_path)

        # check if folder exists
        assert os.path.isdir(path_to_folder), f'Folder not found! [{path_to_folder}]'

        # search files to import (all JSON files in folder)
        files: List[str] = []
        for dirpath, dirs, files in os.walk(path_to_folder):
            json_files = [ f for f in files if f.endswith( ('.json') ) ]
            files = json_files

        # check if any file found
        assert len(files) > 0, f'No files to import found! [{path_to_folder}]'
        files.sort()

        #print(files)

        for file in files:
            index = files.index(file)
            path_to_file = os.path.join(path_to_folder, file)
            data = SqliteDbManager.get_file_content(path_to_file)







        

    @staticmethod
    def import_generated(environments: EEnvironment):
        #TODO:
        pass

    @staticmethod
    def __insert(source: ESource, order: int, environments: EEnvironment, time_created: str, data: str):
        # CREATE TABLE TestData (RecordSource INTEGER, RecordOrder INTEGER, RecordEnvironments INTEGER, TimeCreated TEXT, RecordData TEXT, PRIMARY KEY (RecordSource, RecordOrder));
        pass