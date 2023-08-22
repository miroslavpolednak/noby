import os
import shutil
import subprocess
from datetime import datetime

urls = ['dev_url', 'fat_url', 'sit_url', 'uat_url']
source_file_path = 'health_checks_playground.py'
report_directory = 'D:\\app\\httpd\\html'

# Seznam cest k souborům
destination_file_paths = []

for url in urls:
    destination_file_path = f"{url}_health_checks_playground.py"
    shutil.copy(source_file_path, destination_file_path)

    # Přidání @pytest.mark.parametrize("url_name", ["$url"])
    with open(destination_file_path, 'r') as file:
        lines = file.readlines()

    output = []
    for line in lines:
        stripped_line = line.rstrip()
        if stripped_line.startswith('def test'):
            output.append('@pytest.mark.parametrize("url_name", ["' + url + '"])')
        output.append(stripped_line)

    with open(destination_file_path, 'w') as file:
        file.write('\n'.join(output))

    # Přidání cesty k souboru do seznamu
    destination_file_paths.append(destination_file_path)

# Získání aktuálního času a formátování do požadovaného formátu
current_time = datetime.now().strftime("%H:%M:%S")
report_title = f"Test_time: {current_time}"

# Spuštění všech testů najednou a generování jednoho reportu
report_path = os.path.join(report_directory, "multi_url_report.html")
subprocess.call(['pytest', *destination_file_paths, "--html-report=" + report_path, "--title=" + report_title])

# Smazání všech kopií
for path in destination_file_paths:
    os.remove(path)
