import os
from typing import List
from logging import Logger, basicConfig, getLogger, LogRecord, Filter, Formatter, FileHandler, StreamHandler, INFO

class LogFilter(Filter):
    # https://docs.python.org/3/library/logging.html#filter-objects

    def filter(self, record: LogRecord):
        # hasLogger = Log.hasLogger(record.name)
        # print (f'Logger: {record.name}')
        return True

class Log():

    __logs_folder_path: str = None

    __file_name_extension = '.log'
    __default_formatter = Formatter('%(asctime)s.%(msecs)03d | %(levelname)-8s | %(name)s | %(message)s', '%d.%m.%y %H:%M:%S')
    __default_level = INFO

    __loggers_by_name = dict()
    __file_handlers_by_name = dict()

    @staticmethod
    def __get_logs_folder_path() -> str:
        if Log.__logs_folder_path is None:
            from common import find_folder_tests_be
            Log.__logs_folder_path = os.path.join(find_folder_tests_be(), 'LOGS')

            # create folder if not exists
            if not os.path.exists(Log.__logs_folder_path):
                os.makedirs(Log.__logs_folder_path)

        return Log.__logs_folder_path
    
    @staticmethod
    def __to_file_path(file_name: str) -> str:
        return os.path.join(Log.__get_logs_folder_path(), file_name)

    @staticmethod
    def config():
        basicConfig(
            format=Log.__default_formatter._fmt,
            datefmt=Log.__default_formatter.datefmt,
            level=Log.__default_level,
            handlers=[
                FileHandler(Log.__to_file_path('MainLog.log')  , 'a', 'utf-8'),
                StreamHandler()
            ]
        )

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

        Log.__file_handlers_by_name.keys()
        
        for logger in loggers:

            # create dict of logger's file handlers by file name            
            handlers_by_file_name = { os.path.basename(h.baseFilename) : h for h in logger.handlers if isinstance(h, FileHandler)}

            # add file handler to logger if not found
            for log_file_name in Log.__file_handlers_by_name.keys():
                if log_file_name not in handlers_by_file_name.keys():
                    logger.handlers.append(Log.__file_handlers_by_name[log_file_name])
            
            # remove file handler from logger if not found
            for log_file_name in handlers_by_file_name.keys():
                if log_file_name not in Log.__file_handlers_by_name.keys():
                    logger.handlers.remove(handlers_by_file_name[log_file_name])

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
    def __to_file_name_with_extension(file_name: str) -> str:
        if file_name.endswith(Log.__file_name_extension):
            return file_name
        return file_name + Log.__file_name_extension


# Log.getLogger('MyLog').debug('My DEBUG')
# Log.getLogger('MyLog').info('My INFO')
# Log.getLogger('MyLog').warn('My WARN')
# Log.getLogger('MyLog').warning('My WARNING')
# Log.getLogger('MyLog').error('My ERROR')
# Log.getLogger('MyLog').fatal('My FATAL')
# Log.getLogger('MyLog').critical('My CRITICAL')

# https://realpython.com/python-logging/
