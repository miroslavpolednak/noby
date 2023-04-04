from .Log import Log
from logging import INFO

class LogFileContext:

    def __init__(self, log_file_name: str):
        self.__log = Log.getLogger(self.__class__.__name__)
        self.__log_file_name = log_file_name
        self.__separator = '-' * 300

    def __enter__(self):
        
        Log.fileHandlerAdd(self.__log_file_name)

        self.__log.log(INFO, self.__separator)
        self.__log.log(INFO, f'FILE CONTEXT ADD -> {self.__log_file_name}')
        self.__log.log(INFO, self.__separator)

        return self.__log_file_name

    def __exit__(self, exc_type, exc_val, exc_tb):

        self.__log.log(INFO, self.__separator)
        self.__log.log(INFO, f'FILE CONTEXT REMOVE <- {self.__log_file_name}')
        self.__log.log(INFO, self.__separator)

        Log.fileHandlerRemove(self.__log_file_name)