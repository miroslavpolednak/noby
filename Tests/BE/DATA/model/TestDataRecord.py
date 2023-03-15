from datetime import datetime
from .ESource import ESource
from .Options import Options

class TestDataRecord():
    def __init__(self, source: ESource, order: int, options: Options, time_created: datetime, source_name: str):
        self.__source = source
        self.__order = order
        self.__options = options
        self.__time_created = time_created
        self.__source_name = source_name

    @property
    def source(self) -> ESource:
        return self.__source

    @property
    def order(self) -> int:
        return self.__order

    @property
    def options(self) -> Options:
        return self.__options

    @property
    def time_created(self) -> datetime:
        return self.__time_created

    @property
    def source_name(self) -> str:
        return self.__source_name

    def __str__ (self):
        return f'TestDataRecord [source: {self.__source.name} | order: {self.__order} | options: {self.__options}, time_created: {self.__time_created.isoformat()}, source_name: {self.__source_name}]'