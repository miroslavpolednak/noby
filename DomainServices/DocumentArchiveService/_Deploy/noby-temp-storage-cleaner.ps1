$limit = (Get-Date).AddDays(-3)
# $limit = (Get-Date).AddMinutes(-1)


$path = "D:\www\noby_temp_storage"

# Delete files older than the $limit.
Get-ChildItem -Path $path -Recurse -Force | Where-Object { !$_.PSIsContainer -and $_.CreationTime -lt $limit } | Remove-Item -Force

return 0