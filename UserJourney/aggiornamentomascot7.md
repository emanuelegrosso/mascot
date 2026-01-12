# Specifiche Prototipo – Versione 2.3 (Gestione Natanti, Stati, Pagamenti e Tariffe)  
**Destinatario**: Agente AI per progettazione UI/UX, logica applicativa e modello dati  
**Data riferimento**: 12 gennaio 2026  

---

## 1. Classificazione Natanti

### 1.1 Tipologia natante
I natanti sono suddivisi in due categorie:

| Tipo | Codice natante | Esempi | Capienza massima |
|------|----------------|--------|------------------|
| Gommone | Inizia con `L` (es. L01, L02, ...) | Tutti i gommoni | Variabile per modello (vedi §3.2) |
| Barca escursione | `Enteara`, `Riva` | Escursioni con skipper | Enteara: 10 persone, Riva: 12 persone |

> Nota: Il sistema deve distinguere tra “noleggio” (gommoni) e “escursione” (barche con skipper e pranzo a bordo).

---

## 2. Tabella `skipper`

*(Invariata rispetto alla versione 2.2 — vedi sezione precedente)*

---

## 3. Sezione: Nuova Prenotazione

### 3.1 Selezione natante
- Dropdown che mostra:
  - Tutti i **gommoni** (con codice `Lxx` + potenza)
  - Tutte le **barche escursione** (`Enteara`, `Riva`)
- Per ogni natante:
  - Mostrare potenza (per gommoni) o capienza (per barche escursione)
  - Mostrare prezzo standard in base al mese selezionato (vedi §5)

### 3.2 Calcolo prezzi (tariffe stagionali)
Prezzi estratti dall’immagine fornita:

#### Gommoni (prezzo giornaliero, benzina esclusa):

| Modello | Maggio | Giugno | Luglio | Agosto | Settembre | Ottobre | Novembre |
|---------|--------|--------|--------|--------|---------|---------|----------|
| 6mt 40CV | 200 € | 250 € | 350 € | 400 € | 300 € | 200 € | 150 € |
| 6mt 100CV | 250 € | 300 € | 400 € | 450 € | 350 € | 300 € | 250 € |
| 7mt 150CV | 300 € | 350 € | 450 € | 550 € | 400 € | 350 € | 300 € |
| 7/7,5mt 200CV | 350 € | 400 € | 500 € | 650 € | 500 € | 400 € | 350 € |
| 8mt 250CV | 400 € | 500 € | 600 € | 850 € | 550 € | 500 € | 400 € |
| 10mt 300CV | 600 € | 800 € | 1000 € | 1200 € | 800 € | 700 € | 600 € |

#### Barche escursione (prezzo a persona, con skipper e pranzo a bordo):

| Mese | Prezzo a persona |
|------|------------------|
| Maggio | 100 € |
| Giugno | 120 € |
| Luglio | 130 € |
| Agosto | 150 € |
| Settembre | 120 € |
| Ottobre | 100 € |
| Novembre | 100 € |

> **Note**:
> - Benzina sempre esclusa (da specificare chiaramente).
> - Prezzo per escursione = `numero_partecipanti × prezzo_per_persona`.

### 3.3 Acconto
- Campo opzionale: **“Acconto versato”**
  - Input numerico (€)
  - Checkbox: “Acconto pagato” → abilita campo importo.
- Il valore dell’acconto viene sottratto dal “Prezzo reale” nella visualizzazione finale.

---

## 4. Calendario Giornaliero – Dettaglio Prenotazione

Ogni blocco prenotazione deve mostrare:

### 4.1 Informazioni principali
- Nome cliente
- Natante (es. `L03 - 6mt 40CV` o `Enteara`)
- Orario di inizio/fine (se applicabile)
- Numero partecipanti (solo per escursioni)
- Prezzo totale (standard e reale)

### 4.2 Stato del gommone (solo per noleggi)
- Label visibile: **“Stato: [Opzionato / Confermato / Ritirato / Riconsegnato]”**
- Modificabile tramite dropdown contestuale (click sulla label o icona ✏️)
- Regole di transizione:
  - Opzionato → Confermato / Annullato
  - Confermato → Ritirato / Annullato
  - Ritirato → Riconsegnato
  - Riconsegnato → Completata
  - Annullata / Completata → stato terminale

### 4.3 Pagamento
- Tasto separato: **“Pagato”** (con icona 💰 o ✅)
- Funzionalità:
  - Click → cambia lo stato della prenotazione da “Confermato” a “Pagata”
  - Se già pagata → mostra “Ricevuta emessa” o “Pagamento confermato”
- Se è stato lasciato un acconto:
  - Mostrare: **“Acconto: X €”** (in evidenza, colore arancione o blu)
  - Calcolare residuo: `Prezzo reale - Acconto`

### 4.4 Skipper
- Icona ⚓ + nome skipper (se presente)
- Non visibile per escursioni (poiché skipper è sempre incluso)

### 4.5 Capienza (solo per escursioni)
- Label: **`X/Y`** (es. `8/10` per Enteara, `11/12` per Riva)
- Avviso rosso se `X > Y`
- Visualizzato anche nel tooltip

---

## 5. Scheda Cliente

*(Invariata rispetto alla versione 2.2 — vedi sezione precedente)*

---

## 6. Regole di Business Aggiornate

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento d’identità richiesto |
| Gommone ≥ 100 CV **senza skipper** | Documento + patente nautica obbligatori |
| Gommone ≥ 100 CV **con skipper** | Solo documento d’identità richiesto |
| Escursione | Documento d’identità richiesto (patente non necessaria) |
| Stato prenotazione | Deve seguire regole di transizione definite |
| Modifica stato | Consentita direttamente dal calendario |
| Acconto | Campo opzionale, calcolato nel residuo |
| Pagamento | Tasto separato, cambia stato in “Pagata” |
| Natanti | Differenziati per tipo: `Lxx` = gommoni, `Enteara/Riva` = escursioni |

---

## 7. Note per l’Agente AI

- Tutti i prezzi devono essere calcolati automaticamente in base al mese selezionato.
- La classificazione dei natanti deve essere hardcoded o caricata da una tabella di configurazione.
- Lo stato “Pagato” è indipendente dallo stato del gommone (es. può essere pagata ma non ancora ritirata).
- Il campo “Acconto” deve essere tracciato separatamente dal pagamento finale.
- I colori degli stati devono essere coerenti con quelli definiti nelle versioni precedenti.
- Priorità UX: chiarezza operativa per lo staff — tutti i campi devono essere rapidamente modificabili e visibili.
- Il sistema deve permettere di filtrare le prenotazioni per stato (es. “Mostra solo quelle non pagate”).