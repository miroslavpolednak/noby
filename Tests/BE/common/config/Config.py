import os
from typing import List
from .IConfig import IConfig
from common.mssql.DbConnection import DbConnection as MsSqlDbConnection
from common.mssql.DbManager import DbManager as MsSqlDbManager
from common.enums.EService import EService
from common.enums.EServiceType import EServiceType

class Config(IConfig):
    def __init__(self):
        self._service_discovery_rows = None
        
    @property
    def env_name(self) -> str:
        """Returns name of environment [DEV, FAT, SIT] """
        return os.getenv("ENV")

    @property
    def server(self) -> str:
        """Returns server"""
        return os.getenv("SERVER")

    @property
    def fe_api_url(self) -> str:
        """Returns FeAPI URL"""
        return os.getenv("FE_API_URL")

    @property
    def is_discovery_db_connection(self) -> MsSqlDbConnection:
        """Returns db connection params for internal service Discovery"""
        return MsSqlDbConnection(
            os.getenv("IS_DISCOVERY_DB_SERVER"),
            os.getenv("IS_DISCOVERY_DB_DATABASE"),
            os.getenv("IS_DISCOVERY_DB_USER"),
            os.getenv("IS_DISCOVERY_DB_PASSWORD")
        )

    def get_service_url(self, servce: EService, type: EServiceType) -> str:
        """Returns grpc url of required service"""

        def load_service_discovery()->List[object]:
            sql_manager = MsSqlDbManager(self.is_discovery_db_connection)
            return sql_manager.exec_query(f"SELECT [ServiceName], [ServiceUrl], [ServiceType] FROM [dbo].[ServiceDiscovery] WHERE [EnvironmentName] = '{self.env_name}'")

        if (self._service_discovery_rows is None):
            self._service_discovery_rows = load_service_discovery()

        svc_discovery_rows = list(filter(lambda i: (i.ServiceName == servce.value and i.ServiceType == type.value), self._service_discovery_rows)) 

        return None if len(svc_discovery_rows) != 1 else svc_discovery_rows[0].ServiceUrl