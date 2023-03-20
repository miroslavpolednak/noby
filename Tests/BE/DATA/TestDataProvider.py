import os, json

from typing import List
from datetime import datetime

from common import Log, SqliteDbManager, find_folder_tests_be, ETestEnvironment, ETestLayer, ETestType, config
from .model.ESource import ESource
from .model.Options import Options
from .model.TestDataEntry import TestDataEntry
from .model.TestDataRecord import TestDataRecord

class TestDataProvider():

    __log = Log.getLogger(__file__)
    __db_manager = SqliteDbManager(os.path.join(find_folder_tests_be(), 'DATA', 'db', 'DB.db'))
    __folder_tests_be: str = find_folder_tests_be()

    __OPTIONS_FILE_NAME = 'options.json'

    @staticmethod
    def reset():
        path_to_sql: str = os.path.join(TestDataProvider.__folder_tests_be, 'DATA', 'db', 'sql_init.sql')
        TestDataProvider.__db_manager.exec_scripts([path_to_sql])

    @staticmethod
    def import_custom() -> List[str]:

        # build path to folder
        path_to_folder: str = os.path.join(TestDataProvider.__folder_tests_be, 'DATA', 'custom')

        # check if folder exists
        assert os.path.isdir(path_to_folder), f'Folder not found! [{path_to_folder}]'

        # search subfolders
        subfolders = [ f for f in os.scandir(path_to_folder) if f.is_dir() ]

        # sort folders
        subfolders.sort(key= lambda f: f.name)

        # search files
        order = 0
        time: str = datetime.now().replace(microsecond=0).isoformat()
        sources: List[str] = []
        for folder in subfolders:
            file_names = [ f.name for f in os.scandir(folder.path) if f.is_file() and f.name.endswith('.json') ]

            if len(file_names) == 0:
                continue

            assert TestDataProvider.__OPTIONS_FILE_NAME in file_names, f'Options file [{TestDataProvider.__OPTIONS_FILE_NAME}] not found in folder [{folder}]'

            # read options
            options_content: str = SqliteDbManager.get_file_content(os.path.join(folder, TestDataProvider.__OPTIONS_FILE_NAME))
            options_json: dict = json.loads(options_content)
            options: Options = Options.from_json(options_json)
            print(options)

            # remove options file
            file_names.remove(TestDataProvider.__OPTIONS_FILE_NAME)

            # sort files
            file_names.sort()

            # import files
            for file_name in file_names:
                order += 1
                path_to_file = os.path.join(folder.path, file_name)
                data_content: str = SqliteDbManager.get_file_content(path_to_file)
                data_json: dict = json.loads(data_content)
                source_name = f'{folder.name}/{file_name}' 
                TestDataProvider.__insert(ESource.Custom, order, data_json, options, time, source_name)
                sources.append(source_name)
        
        return sources
  
    @staticmethod
    def import_generated(time: datetime, records: List[TestDataEntry]):

        time_created = time.replace(microsecond=0).isoformat()

        for r in records:
            TestDataProvider.__insert(ESource.Generator, r.order, r.data_json, r.options, time_created)

    @staticmethod
    def __insert(source: ESource, order: int, data_json: dict, options: Options, time_created: str, source_name: str = None):
        # print('OnInsert', source, order, options, time_created, source_name)
        # CREATE TABLE TestData ([Source] INTEGER NOT NULL, [Order] INTEGER NOT NULL, [DataJson] TEXT NOT NULL, [TestEnvironments] INTEGER NOT NULL, [TestLayers] INTEGER NOT NULL, [TestTypes] INTEGER NOT NULL, [TimeCreated] TEXT NOT NULL, [SourceName] TEXT NULL, PRIMARY KEY ([Source], [Order]));

        param_data_json: str = json.dumps(data_json, indent = 4) 
        param_source_name: str = 'NULL' if source_name is None else f"'{source_name}'"
        
        sql_command: str = f"""INSERT INTO TestData([Source], [Order], [DataJson], [TestEnvironments], [TestLayers], [TestTypes], [TimeCreated], [SourceName]) 
                           VALUES ({source.value}, {order}, '{param_data_json}', {options.environments.value}, {options.layers.value}, {options.types.value}, '{time_created}', {param_source_name});"""

        TestDataProvider.__db_manager.exec_nonquery([sql_command])

    @staticmethod
    def load_records_custom_api(types: ETestType = None) -> List[TestDataRecord]:
        return TestDataProvider.load_records(ESource.Custom, config.environment, ETestLayer.API, types)

    @staticmethod
    def load_records_generated_api(types: ETestType = None) -> List[TestDataRecord]:
        return TestDataProvider.load_records(ESource.Generator, config.environment, ETestLayer.API, types)


    @staticmethod
    def load_records(source: ESource = None, environments: ETestEnvironment = None, layers: ETestLayer = None,  types: ETestType = None) -> List[TestDataRecord]:

        def to_record(row: dict) -> TestDataRecord:
            source = ESource(row['Source'])
            options = Options(ETestEnvironment(row["TestEnvironments"]), ETestLayer(row["TestLayers"]), ETestType(row["TestTypes"]))
            time_created = datetime.fromisoformat(row["TimeCreated"])           
            return TestDataRecord(source, row["Order"], options, time_created, row["SourceName"], row["DataJsonLabel"])

        where_conditions: List[str] = []

        if source is not None:
            where_conditions.append(f'[Source] = {source.value}')

        if environments is not None:
            where_conditions.append(f'[TestEnvironments] & {environments.value} <> 0')

        if layers is not None:
            where_conditions.append(f'[TestLayers] & {layers.value} <> 0')

        if types is not None:
            where_conditions.append(f'[TestTypes] & {types.value} <> 0')

        query: str = "SELECT [Source], [Order], [TestEnvironments], [TestLayers], [TestTypes], [TimeCreated], [SourceName], json_extract([DataJson], '$.label') AS 'DataJsonLabel' FROM TestData"

        if len(where_conditions) > 0:
            where = ' AND '.join(where_conditions)
            query += f' WHERE {where}'

        rows = TestDataProvider.__db_manager.exec_query(query)

        return list(map(to_record, rows))

    @staticmethod
    def load_record_data(source: ESource, order: int) -> dict:

        query: str = f'SELECT [DataJson] FROM TestData WHERE [Source] = {source.value} AND [Order] = {order}'
       
        data_json: dict = TestDataProvider.__db_manager.exec_scalar(query)

        assert data_json is not None, f'Record not found [source: {source.name}, order: {order}]'

        return data_json
