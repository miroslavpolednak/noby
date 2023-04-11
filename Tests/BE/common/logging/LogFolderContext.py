from .Log import Log
from datetime import datetime
from logging import INFO

class LogFolderContext:

    def __init__(self, clear_folders_enter: bool = False):
        self.__log = Log.getLogger(self.__class__.__name__)
        self.__log_folder_name = self.__build_folder_name()
        self.__clear_folders_enter = clear_folders_enter
        self.__separator = '=' * 300

    def __enter__(self):
        Log.subfolderAdd(self.__log_folder_name, self.__clear_folders_enter)

        self.__log.log(INFO, self.__separator)
        self.__log.log(INFO, f'FOLDER CONTEXT ADD -> {self.__log_folder_name} [clear: {self.__clear_folders_enter}]')
        self.__log.log(INFO, self.__separator)

        return self.__log_folder_name

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.__log.log(INFO, self.__separator)
        self.__log.log(INFO, f'FOLDER CONTEXT REMOVE <- {self.__log_folder_name}')
        self.__log.log(INFO, self.__separator)
        
        Log.subfolderRemove(self.__log_folder_name)
        
    def __build_folder_name(self) -> str:
        now = datetime.now()
        return now.strftime("%Y%d%m_%H%M%S")