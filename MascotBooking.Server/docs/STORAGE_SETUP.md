# 📦 Configurazione Storage Cloud per Documenti

Questa guida spiega come configurare lo storage cloud per i documenti dei clienti (Google Drive o Dropbox).

## 🎯 Provider Supportati

- **Dropbox** (Consigliato - più semplice da configurare)
- **Google Drive** (Richiede Service Account)

## 📋 Configurazione Dropbox

### 1. Crea un'App Dropbox

1. Vai su [Dropbox App Console](https://www.dropbox.com/developers/apps)
2. Clicca su "Create app"
3. Scegli:
   - **API**: Scoped access
   - **Type**: Full Dropbox
   - **Name**: MascotBooking (o un nome a tua scelta)
4. Clicca "Create app"

### 2. Genera Access Token

1. Nella pagina dell'app, vai su "Permissions"
2. Assicurati che "files.content.write" e "files.content.read" siano abilitati
3. Vai su "Settings" → "OAuth 2"
4. Genera un "Generated access token"
5. **Copia il token** (lo userai in appsettings.json)

### 3. Configura appsettings.json

```json
{
  "Storage": {
    "Provider": "Dropbox",
    "Dropbox": {
      "AccessToken": "IL_TUO_ACCESS_TOKEN_QUI",
      "BasePath": "/MascotBooking"
    }
  }
}
```

## 📋 Configurazione Google Drive

### 1. Crea un Service Account

1. Vai su [Google Cloud Console](https://console.cloud.google.com/)
2. Crea un nuovo progetto o seleziona uno esistente
3. Vai su "APIs & Services" → "Credentials"
4. Clicca "Create Credentials" → "Service Account"
5. Compila i dettagli e crea il service account
6. Vai su "Keys" → "Add Key" → "Create new key" → JSON
7. **Scarica il file JSON** (contiene le credenziali)

### 2. Condividi la cartella Drive

1. Apri Google Drive
2. Crea una cartella (es. "MascotBooking Documents")
3. Condividi la cartella con l'email del service account (trovabile nel JSON)
4. Assegna il permesso "Editor"
5. Copia l'ID della cartella dall'URL (es. `https://drive.google.com/drive/folders/FOLDER_ID`)

### 3. Configura appsettings.json

```json
{
  "Storage": {
    "Provider": "GoogleDrive",
    "GoogleDrive": {
      "CredentialsJson": "{\"type\":\"service_account\",\"project_id\":\"...\",\"private_key_id\":\"...\",\"private_key\":\"...\",\"client_email\":\"...\",\"client_id\":\"...\",\"auth_uri\":\"...\",\"token_uri\":\"...\",\"auth_provider_x509_cert_url\":\"...\",\"client_x509_cert_url\":\"...\"}",
      "FolderId": "IL_TUO_FOLDER_ID_QUI"
    }
  }
}
```

**⚠️ IMPORTANTE**: Per sicurezza, usa variabili d'ambiente in produzione invece di mettere le credenziali direttamente in appsettings.json!

## 🔒 Configurazione Sicura (Produzione)

### Usa Variabili d'Ambiente

In produzione (es. Render.io), configura le variabili d'ambiente:

**Per Dropbox:**
```
Storage__Provider=Dropbox
Storage__Dropbox__AccessToken=il_tuo_token
Storage__Dropbox__BasePath=/MascotBooking
```

**Per Google Drive:**
```
Storage__Provider=GoogleDrive
Storage__GoogleDrive__CredentialsJson={"type":"service_account",...}
Storage__GoogleDrive__FolderId=il_tuo_folder_id
```

### appsettings.Production.json

```json
{
  "Storage": {
    "Provider": "Dropbox",
    "Dropbox": {
      "AccessToken": "",
      "BasePath": "/MascotBooking"
    }
  }
}
```

Le credenziali verranno lette dalle variabili d'ambiente.

## 🧪 Test

Dopo la configurazione:

1. Avvia l'applicazione
2. Vai su "Scheda Cliente"
3. Carica un documento o una patente
4. Verifica che il file appaia nella cartella configurata (Drive/Dropbox)

## 🐛 Troubleshooting

### Dropbox: "Invalid access token"
- Verifica che il token sia corretto
- Assicurati che l'app abbia i permessi corretti

### Google Drive: "Permission denied"
- Verifica che la cartella sia condivisa con il service account
- Controlla che il JSON delle credenziali sia valido

### File non visibili pubblicamente
- Dropbox: I link condivisi sono pubblici per default
- Google Drive: Il servizio imposta automaticamente i permessi "anyone can view"

## 📝 Note

- I file vengono organizzati in cartelle: `documents/` per documenti, `licenses/` per patenti
- I nomi file includono timestamp per evitare conflitti
- Dimensione massima file: 10MB (configurabile)
