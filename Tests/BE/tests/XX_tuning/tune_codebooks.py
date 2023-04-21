# ----------------------------
import _setup
# ----------------------------


# # --------------------------------------------------------
# # FIND ALL IMPLEMENTED CODEBOOKS
# # --------------------------------------------------------
# import os

# # folder of CodebookService 
# path_to_folder = 'D:\\Users\\992466q\\source\\repos\\OneSolution\\DomainServices\\CodebookService\\Endpoints'

# # search subfolders
# subfolders = [ f for f in os.scandir(path_to_folder) if f.is_dir() ]

# for f in subfolders:
#     print(f"{f.name} = '{f.name}'")


# # --------------------------------------------------------
# # FIND CODEBOOKS NOT IMPLEMENTED ON FEAPI
# # --------------------------------------------------------
# from fe_api.FeAPI import FeAPI
# from fe_api.enums import ECodebook
# from common import EnumExtensions

# #codebooks = EnumExtensions.enum_to_list(ECodebook)
# codebooks_api = []
# for c in ECodebook:
#     result = FeAPI.Codebooks.load_codebook(c)
#     if 'errorCode' in result[0]:
#         continue
#     codebooks_api.append(c)

# for c in ECodebook:
#     in_api = c in codebooks_api
#     output = f"{c.name} = '{c.name}'"

#     if in_api == False:
#         output = f'# not in API   {output}'

#     print(output)



# --------------------------------------------------------
# TEST CODEBOOKS
# --------------------------------------------------------
from typing import List
from fe_api.FeAPI import FeAPI
from fe_api.enums import ECodebook

for c in ECodebook:
    items = FeAPI.Codebooks.get_codebook(c)
    print(f'- codebook: {c} [{len(items)}]')