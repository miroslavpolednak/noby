print("Package: common")

from .config.IConfig import IConfig
from .config.Config import Config as ConfigDefault

from .mssql.DbConnection import DbConnection as MsSqlDbConnection
from .mssql.DbManager import DbManager as MsSqlDbManager

from .sqlite.DbManager import DbManager as SqliteDbManager

from .enums.ETestEnvironment import ETestEnvironment
from .enums.ETestLayer import ETestLayer
from .enums.ETestType import ETestType

from .enums.EService import EService
from .enums.EServiceType import EServiceType

from .helpers.Convertor import Convertor
from .helpers.EnumExtensions import EnumExtensions
from .helpers.DictExtensions import DictExtensions
from .logging.Log import Log

config:IConfig = ConfigDefault()

# looks for '\Tests\BE' folder by folder of this file
def find_folder_tests_be():
    import os
    FOLDER_BE = 'BE'
    folder = os.path.dirname(__file__)
    assert FOLDER_BE in folder, f'Folder [{FOLDER_BE}] not found in current path [{folder}]!'
    i = folder.rindex(FOLDER_BE)
    return folder[0 : i + len(FOLDER_BE)]