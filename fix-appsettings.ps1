$file = "MascotBooking.Server/appsettings.json"
if (Test-Path $file) {
    $content = Get-Content $file -Raw
    $content = $content -replace '"CredentialsJson":\s*"[^"]*"', '"CredentialsJson": "YOUR_GOOGLE_DRIVE_CREDENTIALS_JSON_HERE"'
    $content = $content -replace '"FolderId":\s*"[^"]*"', '"FolderId": "YOUR_GOOGLE_DRIVE_FOLDER_ID_HERE"'
    Set-Content $file -Value $content -NoNewline
}
