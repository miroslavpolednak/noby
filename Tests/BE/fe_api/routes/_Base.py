import json
from logging import WARNING, DEBUG, INFO
from requests import cookies, Response, request

from common import config, Log

class Base():

    __default_login = '990614w' #!?
    __cookies_by_login: dict = dict()

    def __init__(self, route: str):
        self._log = Log.getLogger(f'FeApi.{self.class_name}')
        assert route is not None, f'Route must be provided!'
        self.__route = route

    @property
    def class_name(self) -> object:
        return self.__class__.__name__

    def __build_url(self, route: str):
        return f'{config.fe_api_url}/{self.__route}/{route}'

    def __to_log_level(self, res: Response) -> int:
        log_level: int = INFO if res.status_code == 200 else WARNING
        return log_level

    def __handle_request(self, method: str, route: str, login: str = None, json_data: dict = None) -> Response:

        res: Response = None

        try:

            if (login is None):
                login = Base.__default_login
        
            url = self.__build_url(route)
            cookies = Base.get_cookies(login)
            res = request(url=url, method=method, cookies=cookies, json=json_data)

            log_message: str = f'REQ - {method} {url} [login: {login}]'
            if json_data is not None:
                #log_message += f' [data: {json.dumps(json_data, indent = 4)}]'
                log_message += f' [data: {json_data}]'
            self._log.log(self.__to_log_level(res), log_message)

        except Exception as e:
            message = f'Error occured while FeAPI request [url: {url}, method: {method}, login: {login}]. Error: {str(e)}'
            print(message)
            self._log.error(message)

            raise message
            
        return res

    def __handle_response(self, res: Response) -> dict:        
        content = None if len(res.content) == 0 else json.loads(res.content)
  
        log_message: str = f'RES - {res.status_code}'
        if content is not None:
            #log_message += f' [data: {json.dumps(content, indent = 4)}]'
            log_message += f' [data: {content}]'
        self._log.log(self.__to_log_level(res), log_message)

        return content

    def get(self, route: str, login: str = None) -> dict:
        response = self.__handle_request(method='GET', route=route, login=login)
        return self.__handle_response(response)

    def post(self, route: str, data: dict, login: str = None) -> dict:
        response = self.__handle_request(method='POST', route=route, login=login, json_data=data)
        return self.__handle_response(response)

    def put(self, route: str, data: dict, login: str = None) -> dict:
        response = self.__handle_request(method='PUT', route=route, login=login, json_data=data)
        return self.__handle_response(response)

    def delete(self, route: str, login: str = None) -> dict:
        response = self.__handle_request(method='DELETE', route=route, login=login)
        return self.__handle_response(response)

    @staticmethod
    def get_cookies(login: str) -> cookies.RequestsCookieJar:

        if (login not in Base.__cookies_by_login):
            url = f'{config.fe_api_url}/users/signin'
            data = dict( login = login )
            response = request(url = url, method='POST', json = data)
            Base.__cookies_by_login[login] = response.cookies
        
        return Base.__cookies_by_login[login]
