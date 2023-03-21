from typing import List
from ._Base import Base
from fe_api.enums.ECodebook import ECodebook

class Codebooks(Base):

    def __init__(self):
        super().__init__(route='codebooks')
        self.__codebooks: dict = None

    def get_codebook(self, codebook: ECodebook) -> List[dict]:
        # print(f'FeAPI.Codebooks [codebook: {codebook}]')

        if (self.__codebooks is None):
            # codebooks separated (AcademicDegreesAfter,AcademicDegreesBefore,BankCodes,...)
            param_q = ','.join( list(e.value for e in ECodebook))
            codebooks = self.get(f'get-all?q={param_q}')

            self.__codebooks = dict()
            for c in codebooks:
                code = c['code']
                items = c['codebook']
                self.__codebooks[code] = items

        if (codebook.value in self.__codebooks.keys()):
            return self.__codebooks[codebook.value]

        raise ValueError(f"Codebook '{codebook.value}' not found! [{','.join(self.__codebooks.keys()) } ]")

    def load_codebook(self, codebook: ECodebook) -> List[dict]:        
        return self.get(f'get-all?q={codebook.value}')