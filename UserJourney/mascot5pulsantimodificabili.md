# Specifiche Prototipo – Versione 2.2 (Stato Noleggio Modificabile)  
**Destinatario**: Agente AI per progettazione UI/UX, logica applicativa e modello dati  
**Data riferimento**: 12 gennaio 2026  

---

## 1. Nuova Entità: Tabella `skipper`

### 1.1 Struttura della tabella
La base dati deve includere una tabella dedicata agli skipper:

| Campo | Tipo | Obbligatorio | Descrizione |
|------|------|--------------|-------------|
| `id_skipper` | UUID / intero autoincrementale | Sì | Identificativo univoco |
| `nome` | Testo | Sì | Nome dello skipper |
| `cognome` | Testo | Sì | Cognome dello skipper |
| `telefono` | Testo / telefono | Sì | Recapito principale |
| `email` | Email | No | Indirizzo email (opzionale) |
| `numero_documento` | Testo | Sì | Es. “AB1234567” |
| `tipo_documento` | Enum / testo | Sì | Valori: `Carta d’identità`, `Passaporto`, `Patente di guida` |
| `numero_patente_nautica` | Testo | Sì | Numero ufficiale della patente nautica |
| `data_scadenza_patente` | Data | Sì | Data di scadenza della patente nautica |
| `note` | Testo lungo | No | Informazioni aggiuntive |
| `attivo` | Booleano | Sì | `true` = disponibile per assegnazione |

> Tutti i documenti devono essere tracciati per conformità legale e assicurativa.

### 1.2 Gestione
- Interfaccia CRUD per gestione skipper.
- Alert se patente in scadenza (< 30 giorni).

---

## 2. Sezione: Nuova Prenotazione

### 2.1 Selezione servizio e date
- Supporto a **escursioni** e **noleggio gommoni**.
- Selezione **multi-data** con calendario integrato.
- Partecipanti: `Adulti` (obbligatorio), `Bambini` (facoltativo).

### 2.2 Calcolo prezzi
- **Prezzo standard applicato**: calcolato automaticamente, sola lettura.
- **Prezzo reale**: campo numerico modificabile, precompilato con il valore standard.

### 2.3 Dati cliente e documenti
- Campi obbligatori: `Telefono`, `Documento d’identità`.
- `Patente nautica`: obbligatoria **solo** per gommoni ≥100 CV **senza skipper**.

### 2.4 Skipper
- Checkbox: **“Noleggio con skipper”** → abilita dropdown con skipper attivi.
- Se selezionato:
  - Cliente **non necessita di patente** anche per gommoni ≥100 CV.
  - Costo dello skipper aggiunto automaticamente al prezzo standard.

---

## 3. Calendario delle Prenotazioni

### 3.1 Modalità di visualizzazione
Quattro viste selezionabili:
- Giornaliera
- Settimanale
- Mensile
- Annuale

### 3.2 Codifica cromatica
Colori distinti per tipologia:

| Tipo | Potenza | Colore |
|------|--------|--------|
| Escursione | — | `#9370DB` (Viola) |
| Gommone | 40 CV | `#1E90FF` (Blu) |
| Gommone | 100 CV | `#DC143C` (Rosso) |
| Gommone | 150 CV | `#32CD32` (Verde) |
| Gommone | 200 CV | `#FF8C00` (Arancione scuro) |
| Gommone | 250 CV | `#8B4513` (Marrone) |
| Gommone | 400 CV | `#2F2F2F` (Grigio scuro) |

### 3.3 Indicatori di carico (viste mensile/annuale)
Mostrare conteggi suddivisi per tipologia, es.:  
`Esc:2 | 40:1 | 100:1`

### 3.4 Capienza escursioni
- Label `X/Y` (es. `6/10`) visibile in vista giornaliera e tooltip.
- Avviso se capienza superata.

### 3.5 Skipper nel calendario
- Icona ⚓ o etichetta “+ Skipper” su ogni prenotazione con skipper assegnato.
- Nome skipper visibile nel dettaglio.

### 3.6 ✨ **Stato del noleggio – Campo modificabile**
- Ogni prenotazione nel calendario (in qualsiasi vista) deve mostrare chiaramente il suo **stato attuale**.
- **Stati supportati**:
  - `Opzionata`
  - `Confermata`
  - `Pagata`
  - `Ritirato` (solo per gommoni)
  - `Rientrato` (solo per gommoni)
  - `Completata`
  - `Annullata`

- **Comportamento**:
  - Il campo “Stato” deve essere **direttamente modificabile** dall’interfaccia del calendario.
  - Modalità di modifica:
    - **Click sulla label dello stato** → apre un dropdown contestuale con gli stati validi.
    - Oppure: icona “✏️” accanto allo stato → permette modifica rapida.
  - La modifica deve essere **salvata immediatamente** (con feedback visivo: check verde o messaggio “Salvato”).
  - **Regole di transizione** (logica applicativa):
    - Da `Opzionata` → può andare a `Confermata`, `Annullata`
    - Da `Confermata` → può andare a `Pagata`, `Annullata`
    - Da `Pagata` → può andare a `Ritirato` (gommoni) o `Completata` (escursioni)
    - Da `Ritirato` → solo a `Rientrato`
    - Da `Rientrato` → solo a `Completata`
    - `Annullata` e `Completata` sono stati terminali (non modificabili ulteriormente)

- **Visualizzazione**:
  - Lo stato deve essere sempre visibile nel blocco prenotazione (es. badge colorato).
  - Colori suggeriti:
    - `Opzionata`: grigio chiaro
    - `Confermata`: blu
    - `Pagata`: verde
    - `Ritirato`: arancione
    - `Rientrato`: viola
    - `Completata`: verde scuro
    - `Annullata`: rosso

---

## 4. Scheda Cliente

### 4.1 Campi anagrafici
- Data e luogo di nascita, domicilio, codice fiscale, documento, tipo documento.

### 4.2 Ultima prenotazione
- Dettaglio sintetico: data, servizio, prezzo, stato.

### 4.3 Prenotazioni in corso
- Elenco di tutte le prenotazioni future non concluse.
- Ogni riga cliccabile → apre la data corrispondente nel calendario.
- Mostra: data, servizio, stato, skipper (se presente).

---

## 5. Regole di Business Aggiornate

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento richiesto |
| Gommone ≥ 100 CV **senza skipper** | Documento + patente nautica |
| Gommone ≥ 100 CV **con skipper** | Solo documento richiesto |
| Skipper selezionato | Deve essere attivo e con patente valida |
| Stato prenotazione | Deve seguire regole di transizione definite |
| Modifica stato | Consentita direttamente dal calendario in tempo reale |

---

## 6. Note per l’Agente AI

- Il campo “Stato” è centrale per la gestione operativa: deve essere **sempre visibile e rapidamente modificabile**.
- Le transizioni di stato devono essere validate lato server per evitare passaggi illegali.
- Tutti i dati personali (clienti e skipper) sono soggetti a GDPR.
- La legenda degli stati (con colori) deve essere accessibile in tutte le viste del calendario.
- L’interfaccia deve fornire feedback immediato dopo la modifica dello stato (es. animazione, messaggio temporaneo).
- Priorità UX: efficienza per lo staff, non estetica promozionale.