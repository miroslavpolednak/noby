from .Log import Log
from datetime import datetime
from logging import DEBUG

class LogFolderContext:

    def __init__(self, clear_folders_enter: bool = False):
        self.__log = Log.getLogger(self.__class__.__name__)
        self.__log_folder_name = self.__build_folder_name()
        self.__clear_folders_enter = clear_folders_enter

    def __enter__(self):
        Log.setSubfolder(self.__log_folder_name, self.__clear_folders_enter)
        self.__log.log(DEBUG, f'CONTEXT FOLDER ADD -> {self.__log_folder_name} [clear: {self.__clear_folders_enter}]')
        return self.__log_folder_name

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.__log.log(DEBUG, f'CONTEXT FOLDER REMOVE <- {self.__log_folder_name}')
        Log.setSubfolder(None)

    def __build_folder_name(self) -> str:
        now = datetime.now()
        return now.strftime("%Y%d%m_%H%M%S")