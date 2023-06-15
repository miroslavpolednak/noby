# Získání aktuálního času a formátování do požadovaného formátu
$current_time = Get-Date -Format "HH:mm:ss"

# Vytvoření názvu souboru
$report_title = "HealthCheck_time: $current_time"
$report_file = "./report.html"

# Spuštění pytestu s vytvořeným názvem souboru a názvem reportu
pytest '..\..\sms.py::test_sms' "--html-report=$report_file" "--title=$report_title"
