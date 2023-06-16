$urls = 'dev_url', 'fat_url', 'sit_url', 'uat_url'
$source_file_path = 'health_checks.py'
$report_directory = 'D:\app\httpd\html'
$destination_file_paths = @()

foreach ($url in $urls) {
    $destination_file_path = "${url}_health_checks.py"
    Copy-Item -Path $source_file_path -Destination $destination_file_path

    # Přidání @pytest.mark.parametrize("url_name", ["$url"])
    $lines = Get-Content $destination_file_path
    $output = @()
    foreach ($line in $lines) {
        if ($line -match "^def test") {
            $output += '@pytest.mark.parametrize("url_name", ["' + $url + '"])'
        }
        $output += $line.TrimEnd()
    }

    $output | Set-Content $destination_file_path

    # Přidání cesty k souboru do seznamu
    $destination_file_paths += $destination_file_path
}

# Získání aktuálního času a formátování do požadovaného formátu
$current_time = Get-Date -Format "HH:mm:ss"
$report_title = "Test_time: $current_time"

# Spuštění všech testů najednou a generování jednoho reportu
$report_path = Join-Path -Path $report_directory -ChildPath "multi_url_report.html"
$pytest_args = @($destination_file_paths, "--html-report=$report_path", "--title=$report_title")
& pytest $pytest_args

# Smazání všech kopií
foreach ($path in $destination_file_paths) {
    Remove-Item -Path $path
}
