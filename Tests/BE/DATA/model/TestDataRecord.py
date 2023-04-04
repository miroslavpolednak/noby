from datetime import datetime
from .ESource import ESource
from .Options import Options

class TestDataRecord():

    def __init__(self, source: ESource, order: int, options: Options, time_created: datetime, source_name: str, data_json_label: str):
        self.__source = source
        self.__order = order
        self.__options = options
        self.__time_created = time_created
        self.__source_name = source_name
        self.__data_json_label = data_json_label

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

    @property
    def data_json_label(self) -> str:
        return self.__data_json_label

    @property
    def test_label(self) -> str:
        return self.__create_test_label()

    @property
    def log_file_name(self) -> str:
        file_name: str = f'{self.__source.name}_{str(self.__order).zfill(3)}'
        # invalid file name char '/' !
        # if self.__source_name is not None:
        #     file_name += f'_[{self.__source_name}]'
        return file_name

    def __create_test_label(self) -> str:
        label: str = f'{self.__order}'

        if self.__data_json_label is not None:
            label += f': {self.__data_json_label}'

        if self.__source_name is not None:
            label += f' [{self.__source_name}]'

        return label

    def __str__ (self):
        return f'TestDataRecord [source: {self.__source.name} | order: {self.__order} | options: {self.__options}, time_created: {self.__time_created.isoformat()}, source_name: {self.__source_name}, data_json_label: {self.__data_json_label}]'