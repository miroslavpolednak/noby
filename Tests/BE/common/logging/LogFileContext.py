from .Log import Log

class LogFileContext:
    def __init__(self, log_file_name: str):
        self.__log_file_name = log_file_name

    def __enter__(self):
        Log.fileHandlerAdd(self.__log_file_name)
        return self.__log_file_name

    def __exit__(self, exc_type, exc_val, exc_tb):
        Log.fileHandlerRemove(self.__log_file_name)