# 🐳 Script Docker per MascotBooking

Questa cartella contiene script batch per gestire facilmente Docker in locale.

## 📋 Script Disponibili

### `docker-update-run.bat` ⭐ (Principale)
**Aggiorna e avvia il container Docker**

Esegue automaticamente:
1. Ferma il container esistente (se presente)
2. Rimuove il container vecchio
3. Ricostruisce l'immagine Docker
4. Avvia il nuovo container
5. Mostra lo stato e i log

**Uso**: Doppio click su `docker-update-run.bat`

**Output**: Container disponibile su http://localhost:5000

---

### `docker-stop.bat`
**Ferma il container Docker**

**Uso**: Doppio click su `docker-stop.bat`

---

### `docker-logs.bat`
**Visualizza i log del container in tempo reale**

**Uso**: Doppio click su `docker-logs.bat`
**Per uscire**: Premi `Ctrl+C`

---

### `docker-clean.bat`
**Pulisce risorse Docker (stop + remove container)**

**Uso**: Doppio click su `docker-clean.bat`

---

## 🚀 Quick Start

1. **Prima volta o dopo modifiche al codice**:
   - Esegui `docker-update-run.bat`
   - Attendi il completamento del build
   - Apri http://localhost:5000

2. **Per vedere i log**:
   - Esegui `docker-logs.bat`

3. **Per fermare**:
   - Esegui `docker-stop.bat`

## ⚙️ Configurazione

Gli script usano queste impostazioni (modificabili negli script):
- **Container Name**: `mascotbooking-server`
- **Image Name**: `mascotbooking:latest`
- **Porta Locale**: `5000` (mappata alla porta 8080 del container)
- **Volume**: `mascot-data` (persistenza database SQLite)

## 📝 Note

- Assicurati che **Docker Desktop** sia avviato prima di eseguire gli script
- Il primo build può richiedere alcuni minuti (download immagini .NET)
- I build successivi saranno più veloci grazie alla cache Docker
- Il database SQLite viene salvato nel volume Docker `mascot-data`

## 🐛 Troubleshooting

### "Docker is not running"
- Avvia Docker Desktop
- Attendi che Docker sia completamente avviato

### "Port 5000 is already in use"
- Cambia la porta nello script (variabile `PORT`)
- Oppure ferma il processo che usa la porta 5000

### "Build failed"
- Controlla che il Dockerfile sia presente in `MascotBooking.Server/Docker/`
- Verifica che tutti i file necessari siano presenti

### "Container name already in use"
- Esegui `docker-clean.bat` per rimuovere il container esistente
- Oppure modifica il nome del container nello script
