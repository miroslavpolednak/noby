from common.mssql.DbConnection import DbConnection as MsSqlDbConnection
from common.enums.EEnvironment import EEnvironment
from common.enums.EService import EService
from common.enums.EServiceType import EServiceType

class IConfig:
    
    @property
    def env_name(self) -> str:
        """Returns name of environment [DEV, FAT, SIT] """
        return None

    @property
    def environment(self) -> EEnvironment:
        """Returns environment [DEV, FAT, SIT] """
        return None

    @property
    def server(self) -> str:
        """Returns server"""
        return None
    
    @property
    def is_discovery_db_connection(self) -> MsSqlDbConnection:
        """Returns db connection params for internal service Discovery"""
        return None

    def get_service_url(self, servce: EService, type: EServiceType) -> str:
        """Returns grpc url of required service"""
        return None
