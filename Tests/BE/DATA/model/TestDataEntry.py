from .Options import Options

class TestDataEntry():
    def __init__(self, order: int, data_json: dict, options: Options):
        self.__order = order
        self.__data_json = data_json
        self.__options = options

    @property
    def order(self) -> int:
        return self.__order

    @property
    def data_json(self) -> dict:
        return self.__data_json

    @property
    def options(self) -> Options:
        return self.__options