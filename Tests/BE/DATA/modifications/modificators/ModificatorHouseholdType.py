from business.codebooks import EHouseholdType

from ._Modificator import Modificator

class ModificatorHouseholdType(Modificator):

    def __init__(self):
        super().__init__(regex='^HouseholdType\(type=\w{3,100}\)')

    def modify(self, type: str) -> int:
        self.check_enum_name(EHouseholdType, type)
        enum = EHouseholdType[type]
        return enum.value