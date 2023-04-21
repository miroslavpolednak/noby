from business.codebooks import EProductType

from ._Modificator import Modificator

class ModificatorProductType(Modificator):

    def __init__(self):
        super().__init__(regex='^ProductType\(type=\w{3,100}\)')

    def modify(self, type: str) -> int:
        self.check_enum_name(EProductType, type)
        enum = EProductType[type]
        return enum.value