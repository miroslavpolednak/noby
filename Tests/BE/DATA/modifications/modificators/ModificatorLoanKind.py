from business.codebooks import ELoanKind

from ._Modificator import Modificator

class ModificatorLoanKind(Modificator):

    def __init__(self):
        super().__init__(regex='^LoanKind\(kind=\w{3,100}\)')

    def modify(self, kind: str) -> int:
        self.check_enum_name(ELoanKind, kind)
        enum = ELoanKind[kind]
        return enum.value