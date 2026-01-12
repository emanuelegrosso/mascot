#!/usr/bin/env python3
import os
import re
import json

file_path = "MascotBooking.Server/appsettings.json"
if os.path.exists(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Replace CredentialsJson
    content = re.sub(r'"CredentialsJson":\s*"[^"]*"', '"CredentialsJson": "YOUR_GOOGLE_DRIVE_CREDENTIALS_JSON_HERE"', content)
    
    # Replace FolderId
    content = re.sub(r'"FolderId":\s*"[^"]*"', '"FolderId": "YOUR_GOOGLE_DRIVE_FOLDER_ID_HERE"', content)
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)
