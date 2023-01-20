print("Package: fe_api")

from .FeAPI import FeAPI
from .enums import ECodebook


# import json
# from requests import get, post, cookies, request, Request, Response, session

# fe_api_url = 'https://noby-dev.vsskb.cz/api'
# fe_api_url = 'https://noby-fat.vsskb.cz/api'

# def login(login: str) -> cookies.RequestsCookieJar:
#     url = f'{fe_api_url}/users/signin'
#     data = dict( login = login )
#     response = post(url = url,json = data)
#     return response.cookies

# def handle_request(cookies: cookies.RequestsCookieJar, method: str, url: str, data: dict = None) -> Response:
        
#         print(f'FeAPI.HandleRequest [url: {url}, method: {method}]')

#         headers = { 'Accept': '*/*', 'Accept-Encoding': 'gzip, deflate, br'}

#         res = request(url=url, method=method, cookies=cookies, json=data, headers=headers)

#         # session = session()
#         # resp = session.post(
#         #     url, #webapi_url + "/offer/mortgage",
#         #     cookies=cookies, #cookies=get_cookies,
#         #     json={"productTypeId": 20001,
#         #         "loanKindId": 2001,
#         #         "loanAmount": 2000000,
#         #         "loanDuration": 300,
#         #         "fixedRatePeriod": 60,
#         #         "collateralAmount": 3000000,
#         #         "paymentDay": None,
#         #         "isEmployeeBonusRequested": False,
#         #         "guaranteeDateFrom": data['guaranteeDateFrom'],
#         #         "financialResourcesOwn": 600000,
#         #         "financialResourcesOther": 400000,
#         #         "simulationToggleSettings": True,
#         #         "interestRateDiscount": 0,
#         #         "drawingType": 0,
#         #         "drawingDuration": 0,
#         #         "loanPurposes": [
#         #             {"id": 202, "sum": 1500000},
#         #             {"id": 204, "sum": 500000}
#         #         ],
#         #         "marketingActions":
#         #             {"domicile": True,
#         #             "healthRiskInsurance": True,
#         #             "realEstateInsurance": False,
#         #             "incomeLoanRatioDiscount": False,
#         #             "userVip": False
#         #             },
#         #         "resourceProcessId": "f64d3ea9-185c-4cc7-9d97-13fa31d3d967"}
#         #     )



#         print(f'FeAPI.HandleRequest [url: {url}, method: {method}, status_code: {res.status_code}]]')

#         # Content-Type
#         # Content-Encoding
#         # Accept
        

#         return res

# def get_codebook(cookies: cookies.RequestsCookieJar, codebook:str = 'ProductTypes') -> list:
#     # 'https://noby-dev.vsskb.cz/api/codebooks/get-all?q=FixedRatePeriods'
#     url = f'{fe_api_url}/codebooks/get-all?q={codebook}'
#     response = get(url = url, cookies = cookies)

#     print(f'get_codebook - codebook [{codebook}]')
#     print(f'get_codebook - url [{url}]')
#     print(f'get_codebook - status_code [{response.status_code}]')

#     codebook = json.loads(response.content)

#     return codebook



# o_cookies = login('990614w')

# # ----------------------------------------------------
# import json
# data_str = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}'
# data_json = json.loads(data_str)

# url = f'{fe_api_url}/offer/mortgage'
# response = handle_request(o_cookies, 'POST', url, data_json)
# mortgage = json.loads(response.content)
# print(mortgage)
# print(str(mortgage))

# # ----------------------------------------------------

# codebook:str = 'ProductTypes'
# url = f'{fe_api_url}/codebooks/get-all?q={codebook}'
# product_types = handle_request(o_cookies, 'GET', url)

# # ----------------------------------------------------

# product_types = get_codebook(o_cookies, 'ProductTypes')
# loan_kinds = get_codebook(o_cookies, 'LoanKinds')
# print(product_types)
# print(loan_kinds)

# # ----------------------------------------------------



# # resource/route:
# # - address
# # - admin (AllowAnonymous) ???
# # - case
# # - codebooks
# # - customer
# # - customer-on-sa
# # - document
# # - household
# # - offer
# # - product
# # - sales-arrangement
# # - users
