import logging

class LogFilter(logging.Filter):
    # https://docs.python.org/3/library/logging.html#filter-objects

    def filter(self, record: logging.LogRecord):
        # hasLogger = Log.hasLogger(record.name)
        # print (f'Logger: {record.name}')
        return True

class Log():

    __loggers_by_name = dict()

    @staticmethod
    def config():
        logging.basicConfig(
            # filename='test.log',
            # filemode='a', 
            format='%(asctime)s.%(msecs)03d | %(levelname)-8s | %(name)s | %(message)s',
            datefmt='%d.%m.%y %H:%M:%S',
            level=logging.INFO,
            handlers=[
                logging.FileHandler("test.log", 'a'),
                logging.StreamHandler()
            ]
        )

    @staticmethod
    def getLogger(name: str) -> logging.Logger:

        if name not in Log.__loggers_by_name.keys():
            logger = logging.getLogger(name)
            logger.addFilter(LogFilter())
            Log.__loggers_by_name[name] = logger

        return Log.__loggers_by_name[name]

    @staticmethod
    def hasLogger(name: str) -> bool:
        return name in Log.__loggers_by_name.keys()

Log.config()

# Log.getLogger('MyLog').debug('My DEBUG')
# Log.getLogger('MyLog').info('My INFO')
# Log.getLogger('MyLog').warn('My WARN')
# Log.getLogger('MyLog').warning('My WARNING')
# Log.getLogger('MyLog').error('My ERROR')
# Log.getLogger('MyLog').fatal('My FATAL')
# Log.getLogger('MyLog').critical('My CRITICAL')

# https://realpython.com/python-logging/
