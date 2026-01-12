# 📝 Changelog - Nautica Mascot

Registro delle modifiche e versioni del sistema.

## [1.0.0] - 2026-01-11

### Aggiunto
- ✅ Sistema completo gestione prenotazioni
- ✅ Dashboard con riepilogo giornaliero
- ✅ Form nuova prenotazione con date multiple
- ✅ Calendario 4 viste (Giornaliera, Settimanale, Mensile, Annuale)
- ✅ Scheda cliente completa
- ✅ Ricerca globale unificata
- ✅ Codifica cromatica prenotazioni per tipo/potenza
- ✅ Tooltip informativi
- ✅ Legenda colori accessibile
- ✅ Seed data iniziale (5 clienti, 5 barche, 5 prenotazioni)
- ✅ Seed data dicembre completo
- ✅ Validazione patente nautica (≥100 CV)
- ✅ Calcolo prezzi automatico con stagioni
- ✅ Gestione documenti cliente
- ✅ Upload file documenti (base)
- ✅ Navigazione con parametri query
- ✅ Tema nautico blu ispirato a noleggiogommonipalau.net

### Modificato
- 🔄 Dashboard: card statistiche ora cliccabili
- 🔄 Calendario settimanale: mostra numero persone nel badge
- 🔄 Calendario: colori basati su specifiche (40/100/150 CV + Escursione)

### Documentazione
- 📚 README principale
- 📚 Specifiche funzionali complete
- 📚 Guida tecnica
- 📚 Guida utente
- 📚 Documentazione API/Servizi

### Tecnologie
- .NET 10
- Blazor Server
- Entity Framework Core 10
- SQLite
- Bootstrap 5
- Open Iconic

---

## Note

- Database SQLite locale per sviluppo
- Nessuna autenticazione implementata (da aggiungere)
- Upload file: implementazione base (da completare con storage reale)
