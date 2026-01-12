# 🛥️ Specifiche Prototipo HTML – Noleggio Nautico

## 🎯 Obiettivo
Generare un **prototipo HTML/CSS/JS statico** (single-page) che simuli l’intera **User Journey dello staff** del noleggio, con dati fittizi ma realistici, per validare l’esperienza utente prima dello sviluppo backend.

---

## 🧑‍💼 Utente Target
- Staff del noleggio (non clienti)
- Utilizzo da desktop/tablet in ufficio o al molo
- Priorità: velocità, chiarezza, prevenzione errori

---

## 🗂️ Struttura Pagine Richieste

### 1. **Dashboard (Home)**
- Riepilogo giornaliero:
  - Prenotazioni oggi: `5`
  - Natanti in giro: `3`
  - Fatturato stimato: `€1.200`
  - Prossimi rientri: lista ultime 3 prenotazioni attive

### 2. **Nuova Prenotazione** *(form dinamico)*
- Toggle: **Gommone** / **Escursione**
- Campi condizionali basati sulla selezione
- Dati mockup precaricati per test

### 3. **Calendario Prenotazioni**
- Vista a griglia per data + natante/barca
- Badge colorati per stato
- Azioni rapide (“Segna come Rientrato”)

### 4. **Scheda Cliente**
- Form cliente con caricamento documenti
- Visualizzazione chiara “presente/mancante”

### 5. **Ricerca Intelligente** (integrata in tutte le pagine)
- Campo unico in alto
- Risultati misti: clienti, prenotazioni, barche

---

## 📦 Dati Mockup Precaricati

### Clienti (`clients`)
| ID | Nome | Telefono | Email | Documento | Patente |
|----|------|----------|-------|-----------|---------|
| 1 | Mario Rossi | 3331234567 | mario@email.it | ✅ | ❌ |
| 2 | Luca Bianchi | 3478901234 | luca@gmail.com | ✅ | ✅ |
| 3 | Anna Verdi | 3201122334 | anna@verdi.it | ✅ | ✅ |

### Natanti (`boats`)
| ID | Nome | Tipo | CV | Posti | Prezzo/ora (bassa) | Prezzo/ora (alta) |
|----|------|------|----|-------|---------------------|--------------------|
| L02 | Gommone L02 | Gommone | 150 | 6 | €25 | €30 |
| AUR | Yacht Aurora | Yacht | 300 | 12 | €120 | €150 |

### Escursioni (associate a barche)
- **Yacht Aurora** offre:
  - **Standard**: “Isole Tremiti – 4h” → €600 (esclusiva) / €50 a persona (condivisione, max 12)
  - **Personalizzato**: prezzo variabile

### Prenotazioni (`bookings`)
| ID | Tipo | Cliente | Barca/Natante | Data | Persone | Modalità | Tipo Giro | Prezzo | Stato | Ciclo |
|----|------|--------|----------------|------|--------|----------|----------|--------|--------|--------|
| 101 | Gommone | Mario Rossi | L02 | 12/04/2026 10:00–14:00 | 4 | — | — | €120 | Confermata | Ritirato |
| 102 | Escursione | Luca Bianchi | AUR | 12/04/2026 09:00–13:00 | 6 | Condivisione | Standard | €300 | Confermata | Non ritirato |
| 103 | Escursione | Anna Verdi | AUR | 12/04/2026 15:00–19:00 | 8 | Esclusiva | Personalizzato | €700 | Opzione | — |

---

## 🖥️ Requisiti UI/UX

### Layout Generale
- Header fisso con logo, menu, logout
- Sidebar opzionale (menu verticale)
- Responsive (desktop-first, adatto a tablet)

### Form Nuova Prenotazione – Logica Dinamica
1. **Seleziona tipo**: Gommone / Escursione
2. **Se Gommone**:
   - Mostra: selezione natante, campo persone (opzionale), prezzo calcolato
3. **Se Escursione**:
   - Mostra: selezione barca, modalità (condivisione/esclusiva), numero persone (**obbligatorio**), tipo giro (standard/personalizzato), note, prezzo
   - Se “Condivisione”: mostra posti disponibili in tempo reale
   - Se “Standard”: prezzo = tariffa base
   - Se “Personalizzato”: prezzo modificabile

### Calendario
- Ogni riga = un natante/barca
- Ogni blocco = prenotazione con:
  - Nome cliente
  - Orario
  - Badge: `Confermata` / `Opzione` / `Ritirato` / `Rientrato`
- Click su blocco → apre dettaglio con pulsanti azione

### Documenti Cliente
- Per ogni documento (ID, patente):
  - Se caricato: mostra anteprima + “✓ Presente”
  - Se non caricato: mostra “— Mancante” + pulsante “Carica”

### Ricerca Intelligente
- Campo unico in header
- Risultati live: clienti, prenotazioni, barche
- Esempi validi: “150 CV”, “12/04”, “Mario”, “L02”

---

## ⚙️ Tecnologie Ammesse
- **HTML5**, **CSS3**, **JavaScript vanilla** (nessun framework richiesto)
- **Nessun backend**: tutti i dati sono hardcoded in JS o HTML
- **Single HTML file** preferito (per facilità di condivisione)
- **Stile**: pulito, professionale, tema blu/nautico

---

## ✅ Comportamenti “A Prova di Scemo”
- Non permettere overbooking (slot disabilitati se occupati)
- Bloccare input invalidi (es. 20 persone su gommone da 6)
- Prezzo precompilato ma modificabile
- Stati chiari con colori intuitivi:
  - Blu = confermato
  - Giallo = opzione
  - Rosso = in giro (ritirato)
  - Verde = rientrato

---

## 📤 Output Atteso
Un file **`noleggio-nautico-prototipo.html`** funzionante, apribile offline, che includa:
- Tutte le pagine sopra descritte (anche con tab o ancoraggi)
- Dati mockup precaricati
- Interattività minima (es. toggle form, mostra/nascondi)
- Stile coerente e leggibile

> ✨ **Non serve autenticazione né persistenza**: è un prototipo statico per validare la UI.

---

## 🏁 Note Finali
- Priorità: **chiarezza operativa**, non grafica avanzata
- Il prototipo deve permettere a uno staff reale di **simulare una giornata di lavoro**
- Focus su: creazione prenotazione, gestione rientri, ricerca cliente