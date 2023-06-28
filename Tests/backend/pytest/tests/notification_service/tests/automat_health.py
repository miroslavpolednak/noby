import os

commands = [
    "pytest health_checks.py --ns-url sit_url --html=D:\\app\\httpd\\html\\report_SIT.html",
    "pytest health_checks.py --ns-url dev_url --html=D:\\app\\httpd\\html\\report_DEV.html",
    "pytest health_checks.py --ns-url uat_url --html=D:\\app\\httpd\\html\\report_UAT.html",
    "pytest health_checks.py --ns-url fat_url --html=D:\\app\\httpd\\html\\report_FAT.html"
]

for command in commands:
    os.system(command)
