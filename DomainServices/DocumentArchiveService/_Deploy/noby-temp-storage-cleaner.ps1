# $limit = (Get-Date).AddMinutes(-1)
# $path = "D:\www\noby_temp_storage"

$limit = (Get-Date).AddDays(-3)

$path = "\\fs\noby"

$subfolders = @("DEV", "FAT", "SIT1", "UAT");

# Delete files older than the $limit.
foreach ($folder in $subfolders) {
    $subfolder_path = Join-Path -Path $path -ChildPath $folder
    Get-ChildItem -Path $subfolder_path -Recurse -Force | Where-Object { !$_.PSIsContainer -and $_.CreationTime -lt $limit } | Remove-Item -Force
}

return 0
