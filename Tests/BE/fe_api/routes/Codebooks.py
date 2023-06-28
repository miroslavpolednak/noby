from typing import List
from ._Base import Base
from fe_api.enums.ECodebook import ECodebook

class Codebooks(Base):

    def __init__(self):
        super().__init__(route='codebooks')
        self.__codebooks: dict = dict()

    def get_codebook(self, codebook: ECodebook, use_cache: bool = False) -> List[dict]:

        if (codebook.value not in self.__codebooks.keys()) or (use_cache == False):
            self.__load_codebook(codebook)

        if (codebook.value in self.__codebooks.keys()):
            return self.__codebooks[codebook.value]

        raise ValueError(f"Codebook '{codebook.value}' not found! [{','.join(self.__codebooks.keys()) } ]")

    def __load_codebook(self, codebook: ECodebook):
        codebooks = self.get(f'get-all?q={codebook.value}')
        assert len(codebooks) == 1
        code = codebooks[0]['code']
        items = codebooks[0]['codebook']
        self.__codebooks[code] = items