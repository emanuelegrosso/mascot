# 🛥️ Nautica Mascot - Sistema Gestione Prenotazioni

Sistema di gestione prenotazioni per noleggio nautico sviluppato in **Blazor Server** con **.NET 10** e **SQLite**.

## 📋 Indice Documentazione

- [README.md](./README.md) - Questo file (panoramica generale)
- [SPECIFICHE.md](./SPECIFICHE.md) - Specifiche funzionali complete
- [GUIDA_TECNICA.md](./GUIDA_TECNICA.md) - Documentazione tecnica e architettura
- [GUIDA_UTENTE.md](./GUIDA_UTENTE.md) - Manuale d'uso per lo staff
- [API_SERVIZI.md](./API_SERVIZI.md) - Documentazione servizi e interfacce

## 🚀 Quick Start

### Prerequisiti
- .NET 10 SDK
- Visual Studio 2022 o VS Code (opzionale)

### Installazione

```bash
cd MascotBooking.Server
dotnet restore
dotnet run
```

L'applicazione sarà disponibile su:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### Database

Il database SQLite viene creato automaticamente nella cartella `data/mascot.db` al primo avvio. I dati seed vengono caricati automaticamente se il database è vuoto.

## 🎯 Funzionalità Principali

### 1. Dashboard
- Riepilogo giornaliero prenotazioni
- Natanti in giro
- Fatturato stimato
- Prossimi rientri
- **Ricerca globale** in tutte le entità

### 2. Nuova Prenotazione
- Selezione tipo servizio (Gommone/Escursione)
- Selezione date multiple
- Calcolo automatico prezzi
- Validazione patente nautica (≥100 CV)
- Gestione dati cliente integrata

### 3. Calendario
- **4 viste**: Giornaliera, Settimanale, Mensile, Annuale
- **Codifica cromatica** per tipo e potenza
- Tooltip informativi
- Legenda colori accessibile
- Click per dettagli prenotazione

### 4. Scheda Cliente
- Anagrafica completa
- Gestione documenti (ID, Patente)
- Upload documenti
- Ultima prenotazione

### 5. Ricerca Globale
- Ricerca unificata in Clienti, Prenotazioni, Barche
- Risultati con icone e badge
- Navigazione diretta alle maschere

## 🎨 Design

Il sistema utilizza un tema nautico ispirato al sito [noleggiogommonipalau.net](https://noleggiogommonipalau.net/):
- Colori principali: Blu (#1e3a8a, #3b82f6)
- Bootstrap 5 per componenti UI
- Icone Open Iconic
- Design responsive (desktop-first, tablet-friendly)

## 📊 Codifica Cromatica Prenotazioni

| Tipo | Potenza | Colore | Codice |
|------|---------|--------|--------|
| Gommone | 40 CV | Blu Dodger | #1E90FF |
| Gommone | 100 CV | Rosso Cremisi | #DC143C |
| Gommone | 150 CV | Verde Lime | #32CD32 |
| Gommone | Altre | Arancione | #FFA500 |
| Escursione | N/A | Viola Medio | #9370DB |

## 🗂️ Struttura Progetto

```
MascotBooking.Server/
├── Data/              # Database e seed data
├── Models/            # Modelli entità
├── Services/          # Servizi business logic
├── Pages/             # Componenti Razor
├── Shared/            # Componenti condivisi
├── wwwroot/           # File statici (CSS, JS)
└── docs/              # Documentazione
```

## 🔧 Tecnologie

- **.NET 10** - Framework principale
- **Blazor Server** - UI framework
- **Entity Framework Core** - ORM
- **SQLite** - Database
- **Bootstrap 5** - UI components
- **Open Iconic** - Icone

## 📝 Note

- Sistema sviluppato per uso interno dello staff
- Database locale SQLite (per produzione considerare PostgreSQL)
- Nessuna autenticazione implementata (da aggiungere per produzione)
- Upload documenti: implementazione base (da completare con storage reale)

## 📞 Supporto

Per domande o supporto, consultare la documentazione completa nella cartella `docs/`.
