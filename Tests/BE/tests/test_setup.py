print("Package: tests")

import sys
import os

from dotenv import load_dotenv

FOLDER_BE = 'BE'

# looks for '\Tests\BE' folder by folder of this file
def find_folder_tests_be():
    current_folder = os.path.dirname(__file__)
    assert FOLDER_BE in current_folder, f'Folder [{FOLDER_BE}] not found in current path [{current_folder}]!'
    i = current_folder.rindex(FOLDER_BE)
    return current_folder[0 : i + len(FOLDER_BE)]

PATH_TO_TESTS_BE:str = find_folder_tests_be()
PATH_TO_GRPC:str = f'{PATH_TO_TESTS_BE}\\grpc'

#Load .env variables
load_dotenv(f'{PATH_TO_TESTS_BE}\\.env')

# #Import Stubs CIS
# sys.path.append(f'{PATH_TO_GRPC}/CisTypes')

# #Import Stubs DS
# sys.path.append(f'{PATH_TO_GRPC}/CaseService')
# sys.path.append(f'{PATH_TO_GRPC}/CodebookService')
# sys.path.append(f'{PATH_TO_GRPC}/DocumentArchiveService')
# sys.path.append(f'{PATH_TO_GRPC}/HouseholdService')
# sys.path.append(f'{PATH_TO_GRPC}/OfferService')
# sys.path.append(f'{PATH_TO_GRPC}/ProductService')
# sys.path.append(f'{PATH_TO_GRPC}/SalesArrangementService')
# sys.path.append(f'{PATH_TO_GRPC}/UserService')

# Import Stubs
for node in os.listdir(PATH_TO_GRPC):
    node_path = os.path.join(PATH_TO_GRPC, node)
    if os.path.isdir(node_path):
        sys.path.append(node_path)
