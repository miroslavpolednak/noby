# ----------------------------------------------------------------------------
# SETUP
# ----------------------------------------------------------------------------
def setup():
    import sys
    import os

    from dotenv import load_dotenv

    FOLDER_BE = 'BE'

    # looks for '\Tests\BE' folder by folder of this file
    def find_folder_tests_be():
        current_folder = os.path.dirname(__file__)
        assert FOLDER_BE in current_folder, f'Folder [{FOLDER_BE}] not found in current path [{current_folder}]!'
        i = current_folder.index(FOLDER_BE)
        return current_folder[0 : i + len(FOLDER_BE)]

    PATH_TO_TESTS_BE:str = find_folder_tests_be()
   
    #Load .env variables
    load_dotenv(f'{PATH_TO_TESTS_BE}\.env')

    # Import BE
    sys.path.append(PATH_TO_TESTS_BE)
    for node in os.listdir(PATH_TO_TESTS_BE):
        node_path = os.path.join(PATH_TO_TESTS_BE, node)
        if os.path.isdir(node_path):
            print(node_path)
            sys.path.append(node_path)


    # Import Stubs
    PATH_TO_GRPC:str = f'{PATH_TO_TESTS_BE}\\grpc'
    for node in os.listdir(PATH_TO_GRPC):
        node_path = os.path.join(PATH_TO_GRPC, node)
        if os.path.isdir(node_path):
            print(node_path)
            sys.path.append(node_path)

    # # Import FeApi
    # PATH_TO_FE_API:str = f'{PATH_TO_TESTS_BE}\\fe_api'
    # sys.path.append(PATH_TO_FE_API)
    # for node in os.listdir(PATH_TO_FE_API):
    #     node_path = os.path.join(PATH_TO_FE_API, node)
    #     if os.path.isdir(node_path):
    #         print(node_path)
    #         sys.path.append(node_path)

# ----------------------------------------------------------------------------
setup()
