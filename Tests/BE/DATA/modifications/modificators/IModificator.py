class IModificator:
    
    def __init__(self, name: str):
        self.__name = name

    @property
    def name(self) -> str:
        """Returns name """
        return self.__name

    def modify(self):
        pass