# 🚀 Guida Completa: Configurazione Google Drive

Questa guida ti accompagna passo-passo nella configurazione di Google Drive come storage per i documenti dei clienti.

## 📋 Prerequisiti

- Un account Google
- Accesso a [Google Cloud Console](https://console.cloud.google.com/)

## 🔧 Passo 1: Crea un Progetto Google Cloud

1. Vai su [Google Cloud Console](https://console.cloud.google.com/)
2. Se non hai già un progetto, clicca su **"Select a project"** → **"New Project"**
3. Inserisci un nome (es. "MascotBooking")
4. Clicca **"Create"**
5. Seleziona il progetto appena creato

## 🔑 Passo 2: Abilita Google Drive API

1. Nel menu laterale, vai su **"APIs & Services"** → **"Library"**
2. Cerca **"Google Drive API"**
3. Clicca sul risultato e poi su **"Enable"**
4. Attendi qualche secondo per l'attivazione

## 👤 Passo 3: Crea un Service Account

1. Vai su **"APIs & Services"** → **"Credentials"**
2. Clicca su **"+ CREATE CREDENTIALS"** in alto
3. Seleziona **"Service account"**
4. Compila il form:
   - **Service account name**: `mascotbooking-storage` (o un nome a tua scelta)
   - **Service account ID**: viene generato automaticamente
   - **Description**: (opzionale) "Service account per storage documenti MascotBooking"
5. Clicca **"CREATE AND CONTINUE"**
6. Nella sezione "Grant this service account access to project":
   - **Role**: Seleziona **"Editor"** (o "Storage Admin" se disponibile)
7. Clicca **"CONTINUE"** → **"DONE"**

## 🔐 Passo 4: Genera le Credenziali JSON

1. Nella lista dei Service Accounts, clicca sul service account appena creato
2. Vai alla tab **"KEYS"**
3. Clicca su **"ADD KEY"** → **"Create new key"**
4. Seleziona **"JSON"**
5. Clicca **"CREATE"**
6. **⚠️ IMPORTANTE**: Il file JSON verrà scaricato automaticamente. **SALVALO IN UN POSTO SICURO!**
   - Questo file contiene le credenziali sensibili
   - Non condividerlo mai pubblicamente
   - Non committarlo su Git (è già escluso da .gitignore)

## 📁 Passo 5: Crea e Condividi la Cartella Drive

1. Apri [Google Drive](https://drive.google.com/)
2. Clicca su **"New"** → **"Folder"**
3. Nome della cartella: `MascotBooking Documents` (o un nome a tua scelta)
4. Clicca **"Create"**
5. **Tieni aperta questa cartella** (non chiudere Drive)

### Trova l'Email del Service Account

1. Apri il file JSON scaricato al Passo 4
2. Cerca il campo `"client_email"` (es. `"mascotbooking-storage@progetto-123456.iam.gserviceaccount.com"`)
3. **Copia questa email** - ti servirà tra poco

### Condividi la Cartella

1. Torna su Google Drive nella cartella appena creata
2. Clicca con il tasto destro sulla cartella → **"Share"** (o clicca sull'icona "Condividi")
3. Incolla l'email del service account nel campo "Aggiungi persone e gruppi"
4. **IMPORTANTE**: Cambia il permesso da "Visualizzatore" a **"Editor"**
5. **Deseleziona** "Notifica le persone" (non è necessario)
6. Clicca **"Condividi"** o **"Invia"**

### Ottieni l'ID della Cartella

1. Nella cartella condivisa, guarda l'URL nella barra degli indirizzi
2. L'URL sarà simile a: `https://drive.google.com/drive/folders/1a2b3c4d5e6f7g8h9i0j`
3. **Copia la parte dopo `/folders/`** (es. `1a2b3c4d5e6f7g8h9i0j`)
4. Questo è il **FolderId** che userai nella configurazione

## ⚙️ Passo 6: Configura appsettings.json

Apri il file `appsettings.json` e modifica la sezione Storage:

```json
{
  "Storage": {
    "Provider": "GoogleDrive",
    "GoogleDrive": {
      "CredentialsJson": "INCOLLA_QUI_IL_CONTENUTO_COMPLETO_DEL_FILE_JSON",
      "FolderId": "INCOLLA_QUI_L_ID_DELLA_CARTELLA"
    }
  }
}
```

### Come inserire il JSON

**Opzione 1: Inline (per sviluppo)**
- Apri il file JSON scaricato
- Copia **tutto il contenuto** (dall'inizio alla fine)
- Incollalo tra le virgolette di `"CredentialsJson"` (sostituendo `INCOLLA_QUI_IL_CONTENUTO_COMPLETO_DEL_FILE_JSON`)
- **IMPORTANTE**: Devi "escapare" le virgolette interne. Esempio:
  ```json
  "CredentialsJson": "{\"type\":\"service_account\",\"project_id\":\"mio-progetto\",...}"
  ```

**Opzione 2: File separato (consigliato per produzione)**
- Lascia il JSON in un file separato (es. `google-credentials.json`)
- Modifica `GoogleDriveStorageService.cs` per leggere da file invece che da configurazione
- Oppure usa variabili d'ambiente (vedi sotto)

### Esempio Completo appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Storage": {
    "Provider": "GoogleDrive",
    "GoogleDrive": {
      "CredentialsJson": "{\"type\":\"service_account\",\"project_id\":\"mascotbooking-123456\",\"private_key_id\":\"abc123...\",\"private_key\":\"-----BEGIN PRIVATE KEY-----\\n...\\n-----END PRIVATE KEY-----\\n\",\"client_email\":\"mascotbooking-storage@mascotbooking-123456.iam.gserviceaccount.com\",\"client_id\":\"123456789\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\":\"https://www.googleapis.com/robot/v1/metadata/x509/mascotbooking-storage%40mascotbooking-123456.iam.gserviceaccount.com\"}",
      "FolderId": "1a2b3c4d5e6f7g8h9i0j"
    }
  }
}
```

## 🔒 Configurazione Sicura per Produzione (Render.io)

**⚠️ NON mettere mai le credenziali JSON direttamente in appsettings.json in produzione!**

### Usa Variabili d'Ambiente

Su Render.io (o altre piattaforme), configura queste variabili d'ambiente:

1. **Storage__Provider** = `GoogleDrive`
2. **Storage__GoogleDrive__CredentialsJson** = `{il contenuto completo del JSON come stringa}`
3. **Storage__GoogleDrive__FolderId** = `{l'id della cartella}`

**Come inserire il JSON nelle variabili d'ambiente:**
- Copia tutto il contenuto del file JSON
- Incollalo direttamente nel campo della variabile d'ambiente
- Non serve "escapare" le virgolette (Render.io lo gestisce automaticamente)

### appsettings.Production.json

```json
{
  "Storage": {
    "Provider": "GoogleDrive",
    "GoogleDrive": {
      "CredentialsJson": "",
      "FolderId": ""
    }
  }
}
```

Le credenziali verranno lette automaticamente dalle variabili d'ambiente.

## ✅ Passo 7: Test della Configurazione

1. **Avvia l'applicazione**:
   ```bash
   cd MascotBooking.Server
   dotnet run
   ```

2. **Vai su "Scheda Cliente"** nell'applicazione

3. **Carica un documento di test**:
   - Seleziona o crea un cliente
   - Clicca "Carica File" per un documento
   - Seleziona un file (PDF, JPG, PNG)

4. **Verifica su Google Drive**:
   - Apri la cartella condivisa su Google Drive
   - Dovresti vedere un nuovo file con nome tipo `20260111_143022_documento.pdf`
   - Il file dovrebbe essere visibile pubblicamente

## 🐛 Troubleshooting

### Errore: "Google Drive credentials not configured"
- Verifica che `Provider` sia impostato su `"GoogleDrive"`
- Controlla che `CredentialsJson` contenga il JSON completo e valido
- Assicurati che le virgolette siano "escaped" correttamente se usi inline

### Errore: "Permission denied" o "File not found"
- **Verifica che la cartella sia condivisa** con l'email del service account
- **Controlla che il permesso sia "Editor"** (non "Visualizzatore")
- Verifica che `FolderId` sia corretto (copia l'ID dall'URL della cartella)

### Errore: "API not enabled"
- Vai su Google Cloud Console → APIs & Services → Library
- Cerca "Google Drive API" e verifica che sia abilitata
- Se non lo è, clicca "Enable"

### File non visibili pubblicamente
- Il servizio imposta automaticamente i permessi "anyone can view"
- Se non funziona, verifica manualmente su Drive:
  - Tasto destro sul file → "Share" → "Change to anyone with the link"

### JSON non valido
- Verifica che tutto il JSON sia su una sola riga se usi inline
- Controlla che tutte le virgolette siano "escaped" con `\"`
- Usa un validatore JSON online per verificare la sintassi

## 📝 Note Importanti

- **Sicurezza**: Non committare mai il file JSON delle credenziali su Git
- **Backup**: Conserva una copia sicura del file JSON delle credenziali
- **Organizzazione**: I file vengono organizzati automaticamente:
  - Documenti → `documents/` nella cartella Drive
  - Patenti → `licenses/` nella cartella Drive
- **Nomi file**: I file includono timestamp per evitare conflitti
- **Dimensione max**: 10MB per file (configurabile nel codice)

## 🎯 Prossimi Passi

Dopo aver configurato Google Drive:
1. Testa l'upload di documenti e patenti
2. Verifica che i file siano accessibili pubblicamente
3. Configura le variabili d'ambiente per produzione
4. Monitora l'uso dello storage su Google Cloud Console

## 📚 Risorse Utili

- [Google Drive API Documentation](https://developers.google.com/drive/api)
- [Service Accounts Guide](https://cloud.google.com/iam/docs/service-accounts)
- [Google Cloud Console](https://console.cloud.google.com/)
