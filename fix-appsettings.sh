#!/bin/bash
if [ -f "MascotBooking.Server/appsettings.json" ]; then
    sed -i 's/"CredentialsJson": "[^"]*"/"CredentialsJson": "YOUR_GOOGLE_DRIVE_CREDENTIALS_JSON_HERE"/g' "MascotBooking.Server/appsettings.json"
    sed -i 's/"FolderId": "[^"]*"/"FolderId": "YOUR_GOOGLE_DRIVE_FOLDER_ID_HERE"/g' "MascotBooking.Server/appsettings.json"
fi
