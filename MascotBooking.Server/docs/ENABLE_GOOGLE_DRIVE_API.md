# 🔧 Abilitare Google Drive API

Se ricevi l'errore:
```
Google Drive API has not been used in project [PROJECT_ID] before or it is disabled
```

Segui questi passi per abilitare l'API.

## 📋 Passi per Abilitare l'API

### 1. Vai su Google Cloud Console

Apri questo link diretto per abilitare l'API:
**https://console.cloud.google.com/apis/library/drive.googleapis.com**

Oppure:
1. Vai su [Google Cloud Console](https://console.cloud.google.com/)
2. Seleziona il progetto corretto (dall'URL dell'errore o dal JSON delle credenziali)
3. Nel menu laterale, vai su **"APIs & Services"** → **"Library"**

### 2. Cerca Google Drive API

1. Nella barra di ricerca, digita **"Google Drive API"**
2. Clicca sul risultato **"Google Drive API"**

### 3. Abilita l'API

1. Clicca sul pulsante **"ENABLE"** (Abilita)
2. Attendi qualche secondo per l'attivazione
3. Vedrai un messaggio di conferma quando l'API è abilitata

### 4. Verifica

1. Vai su **"APIs & Services"** → **"Enabled APIs"**
2. Verifica che **"Google Drive API"** sia nella lista
3. Dovrebbe mostrare lo stato **"Enabled"**

## 🔍 Trovare il Progetto Corretto

Se non sei sicuro di quale progetto usare:

1. **Dal JSON delle credenziali**:
   - Apri il file `directions-366009-8edef2395a51.json`
   - Cerca il campo `"project_id"` (es. `"directions-366009"`)

2. **Dall'errore**:
   - L'errore mostra il project ID numerico (es. `886330336929`)
   - Questo è il project number, non l'ID

3. **In Google Cloud Console**:
   - Vai su [Project Selector](https://console.cloud.google.com/projectselector2/home/dashboard)
   - Cerca per nome o ID del progetto
   - Seleziona il progetto corretto

## ⚠️ Nota Importante

- Se hai **più progetti** Google Cloud, assicurati di abilitare l'API nel progetto **corretto** (quello associato al service account)
- Il service account email è: `mascot@directions-366009.iam.gserviceaccount.com`
- Questo service account appartiene al progetto `directions-366009`

## 🔄 Dopo l'Abilitazione

1. **Attendi 1-2 minuti** per la propagazione
2. **Riprova** a caricare un documento nell'applicazione
3. Se l'errore persiste, verifica:
   - Che il progetto selezionato sia corretto
   - Che l'API sia effettivamente abilitata
   - Che il service account abbia i permessi corretti

## 🐛 Troubleshooting

### "API already enabled"
- L'API potrebbe essere abilitata in un altro progetto
- Verifica di essere nel progetto corretto

### "Permission denied"
- Assicurati di avere i permessi di "Owner" o "Editor" sul progetto
- Contatta l'amministratore del progetto se necessario

### L'errore persiste dopo l'abilitazione
- Attendi 5-10 minuti per la propagazione completa
- Verifica che il service account sia associato al progetto corretto
- Controlla i log su Google Cloud Console → APIs & Services → Dashboard

## 📚 Link Utili

- [Google Drive API Documentation](https://developers.google.com/drive/api)
- [Enable APIs Guide](https://cloud.google.com/endpoints/docs/openapi/enable-api)
- [Google Cloud Console](https://console.cloud.google.com/)
