import os, shutil
from typing import List
from logging import Logger, basicConfig, getLogger, LogRecord, Filter, Formatter, FileHandler, StreamHandler, INFO

# https://realpython.com/python-logging/

class LogFilter(Filter):
    # https://docs.python.org/3/library/logging.html#filter-objects

    def filter(self, record: LogRecord):
        # hasLogger = Log.hasLogger(record.name)
        # print (f'Logger: {record.name}')
        return True


class Log():

    __logs_folder: str = 'LOGS'
    __logs_subfolder: str = None

    __logs_folder_path: str = None

    __file_name_extension = '.log'
    __default_formatter = Formatter('%(asctime)s.%(msecs)03d | %(levelname)-8s | %(name)s | %(message)s', '%d.%m.%y %H:%M:%S')
    __default_level = INFO

    __loggers_by_name = dict()
    __file_handlers_by_name = dict()

    __basic_config_done = False

    @staticmethod
    def __get_logs_folder_path() -> str:
        if Log.__logs_folder_path is None:
            from common import find_folder_tests_be
            Log.__logs_folder_path = os.path.join(find_folder_tests_be(), Log.__logs_folder)

            # create folder if not exists
            if not os.path.exists(Log.__logs_folder_path):
                os.makedirs(Log.__logs_folder_path)

        return Log.__logs_folder_path
    
    @staticmethod
    def __get_logs_subfolder_path() -> str:
        subfolder_path = Log.__get_logs_folder_path() if Log.__logs_subfolder is None else os.path.join(Log.__get_logs_folder_path(), Log.__logs_subfolder)

        # create subfolder if not exists
        if not os.path.exists(subfolder_path):
            os.makedirs(subfolder_path)

        return subfolder_path

    @staticmethod
    def __to_file_path(file_name: str) -> str:
        return os.path.join(Log.__get_logs_subfolder_path(), file_name)

    @staticmethod
    def config():
        if Log.__basic_config_done is True:
            return

        basicConfig(
            format=Log.__default_formatter._fmt,
            datefmt=Log.__default_formatter.datefmt,
            level=Log.__default_level,
            handlers=[
                FileHandler(Log.__to_file_path('MainLog.log')  , 'a', 'utf-8'),
                StreamHandler()
            ]
        )

        Log.__basic_config_done = True

    @staticmethod
    def getLogger(name: str) -> Logger:

        if name not in Log.__loggers_by_name.keys():
            logger = getLogger(name)
            logger.addFilter(LogFilter())
            Log.__reset_file_handlers([logger])
            Log.__loggers_by_name[name] = logger
            Log.config()

        return Log.__loggers_by_name[name]

    @staticmethod
    def __create_file_handler(file_name: str) -> FileHandler:
        file_handler = FileHandler(Log.__to_file_path(file_name), 'a', 'utf-8')
        file_handler.setFormatter(Log.__default_formatter)
        file_handler.setLevel(Log.__default_level)
        return file_handler

    @staticmethod
    def hasLogger(name: str) -> bool:
        return name in Log.__loggers_by_name.keys()

    @staticmethod
    def __reset_file_handlers(loggers: List[Logger] = None):

        if loggers is None:
            loggers = Log.__loggers_by_name.values()
        
        for log_file_name in Log.__file_handlers_by_name.keys():
            Log.__file_handlers_by_name[log_file_name] = Log.__create_file_handler(log_file_name)

        for logger in loggers:
            logger.handlers.clear()
            for log_file_name in Log.__file_handlers_by_name.keys():
                logger.handlers.append(Log.__file_handlers_by_name[log_file_name])


    @staticmethod
    def fileHandlerAdd(file_name: str):
        print(f'log_file_handler_add[{file_name}]')

        log_file_name = Log.__to_file_name_with_extension(file_name)

        if log_file_name in Log.__file_handlers_by_name.keys():
            return

        Log.__file_handlers_by_name[log_file_name] = Log.__create_file_handler(log_file_name)
        Log.__reset_file_handlers()


    @staticmethod
    def fileHandlerRemove(file_name: str):
        print(f'log_file_handler_remove[{file_name}]')

        log_file_name = Log.__to_file_name_with_extension(file_name)

        if log_file_name not in Log.__file_handlers_by_name.keys():
            return

        del Log.__file_handlers_by_name[log_file_name]
        Log.__reset_file_handlers()


    @staticmethod
    def subfolderAdd(subfolder_name: str, delete_others: bool = False):
        assert subfolder_name is not None

        if delete_others is True:
            Log.__delete_subfolders()
    
        Log.__logs_subfolder = subfolder_name

        # add subfolder handler 
        log_file_name = Log.__to_file_name_with_extension(subfolder_name)
        Log.__file_handlers_by_name[log_file_name] = Log.__create_file_handler(log_file_name)

        # reset handlers
        Log.__reset_file_handlers()

        
    @staticmethod
    def subfolderRemove(subfolder_name: str):
        assert subfolder_name is not None

        Log.__logs_subfolder = None

        # remove subfolder handler
        log_file_name = Log.__to_file_name_with_extension(subfolder_name)
        del Log.__file_handlers_by_name[log_file_name]
        
        # reset handlers
        Log.__reset_file_handlers()


    @staticmethod
    def __delete_subfolders():
        logs_folder_path = Log.__get_logs_folder_path()

        # quit if folder not exists
        if not os.path.exists(logs_folder_path):
            return

        subfolders = [ f for f in os.scandir(logs_folder_path) if f.is_dir() ]
        for folder in subfolders:
            shutil.rmtree(folder)

    @staticmethod
    def __to_file_name_with_extension(file_name: str) -> str:
        if file_name.endswith(Log.__file_name_extension):
            return file_name
        return file_name + Log.__file_name_extension

    @staticmethod
    def save_snapshot(file_name, json_string: str):
        file_path = Log.__to_file_path(file_name)
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(json_string)
