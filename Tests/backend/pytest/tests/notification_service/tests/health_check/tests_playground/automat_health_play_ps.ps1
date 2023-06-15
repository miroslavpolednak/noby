$urls = 'dev_url', 'fat_url', 'sit_url', 'uat_url'
$source_file_path = 'health_checks_playground.py'
$report_directory = 'D:\app\httpd\html'

foreach ($url in $urls) {
    $destination_file_path = "${url}_health_checks_playground.py"
    Copy-Item -Path $source_file_path -Destination $destination_file_path

    # Přidání @pytest.mark.parametrize("url_name", ["$url"])
    $lines = Get-Content $destination_file_path
    $output = @()
    foreach ($line in $lines) {
        if ($line.Trim().StartsWith('def test')) {
            $output += '@pytest.mark.parametrize("url_name", ["' + $url + '"])'
        }
        $output += $line
    }
    Set-Content -Path $destination_file_path -Value $output

    # Získání aktuálního času a formátování do požadovaného formátu
    $current_time = Get-Date -Format "HH:mm:ss"
    $report_title = "$url, Test_time: $current_time"

    # Pro každou URL spustíme odpovídající test a vytvoříme html zprávu
    $report_path = Join-Path -Path $report_directory -ChildPath "${url}_report.html"
    pytest "${destination_file_path}" "--ns-url" $url "--html-report=$report_path" "--title=$report_title"

    # Po provedení testu smažeme kopii
    Remove-Item -Path $destination_file_path
}
