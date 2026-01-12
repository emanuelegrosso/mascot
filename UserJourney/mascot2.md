# Specifiche Prototipo – Versione 2  
**Destinatario**: Agente AI per progettazione UI/UX e logica applicativa  
**Data riferimento**: 11 gennaio 2026  

---

## 1. Sezione: Nuova Prenotazione

### 1.1 Selezione del servizio
- L’utente deve poter scegliere tra:
  - **Escursione** (con tariffa per persona)
  - **Noleggio gommone** (con tariffa giornaliera fissa, legata alla potenza)
- Il tipo di servizio selezionato determina i campi obbligatori successivi.

### 1.2 Selezione date multipla
- Il campo “Date” deve permettere la selezione di **una o più date** (non necessariamente consecutive).
- Modalità di input: calendario integrato con selezione multipla (click singoli o range).
- Ogni data selezionata è trattata come un’unità di prenotazione indipendente ai fini del calcolo del prezzo.
- Il numero di partecipanti può essere:
  - **Globale** (unico per tutte le date), se il servizio non richiede variazioni giornaliere, oppure
  - **Per data**, se abilitato (opzionale nella v2; per ora si assume globale).

### 1.3 Partecipanti
- Due campi numerici:
  - `Adulti` (obbligatorio, ≥0)
  - `Bambini` (opzionale, ≥0)
- Validazione: almeno un partecipante totale (adulti + bambini ≥ 1).

### 1.4 Calcolo prezzi
- Ogni escursione/gommone ha un prezzo standard associato:
  - Escursione: prezzo per adulto e per bambino.
  - Gommone: prezzo giornaliero fisso (indipendente dal numero di persone).
- **Prezzo standard applicato**:
  - Campo in sola lettura.
  - Calcolato come:  
    `Totale = Σ [per ogni data: (prezzo unitario × giorni)]`  
    (per escursioni: `(adulti × prezzo_adulto + bambini × prezzo_bambino) × n°_date`)  
    (per gommoni: `prezzo_giornaliero × n°_date`)
- **Prezzo reale**:
  - Campo numerico modificabile.
  - Inizialmente precompilato con il valore del “Prezzo standard applicato”.
  - Può essere modificato liberamente dallo staff (sconti, sovrapprezzi, accordi personalizzati).
  - Non viene sovrascritto automaticamente dopo la prima modifica manuale.

### 1.5 Dati cliente (nella prenotazione)
I seguenti campi devono essere presenti nel form:
- `Telefono` (obbligatorio, formato telefono)
- `Email` (facoltativo, formato email)
- `Documento d’identità` (obbligatorio):
  - Checkbox “Presente” + campo testo per numero/tipo oppure upload file.
- `Patente nautica` (condizionale):
  - Obbligatoria **solo** se il gommone selezionato ha potenza ≥100 CV.
  - Altrimenti, non richiesta (solo documento d’identità).
- `Note` (testo libero, facoltativo)

#### Logica condizionale patente
- Se servizio = **Gommone**:
  - Se potenza < 100 CV → richiesto solo documento d’identità.
  - Se potenza ≥ 100 CV → richiesti **documento + patente nautica**.
- Se servizio = **Escursione** → richiesto solo documento d’identità.
- Mostrare messaggio contestuale dinamico in base alla selezione.

---

## 2. Scheda Cliente

### 2.1 Campi anagrafici aggiuntivi (tutti editabili)
- Data di nascita (date picker)
- Luogo di nascita (testo libero)
- Domicilio (indirizzo completo: via, civico, CAP, città, nazione)
- Codice fiscale (testo, validazione opzionale per formato IT)
- Numero documento (es. “AB1234567”)
- Tipo documento (dropdown: Carta d’identità / Passaporto / Patente di guida)

### 2.2 Ultima prenotazione (sola lettura)
Visualizzare in modo sintetico:
- Data
- Tipo (Escursione / Gommone)
- Nome servizio selezionato
- Prezzo totale
- Stato (Confermata / Pagata / Annullata / In attesa)

### 2.3 Comportamento
- I dati della scheda cliente devono essere **riutilizzabili** durante la creazione di una nuova prenotazione (es. ricerca cliente → popolazione automatica dei campi).
- Tutti i campi devono essere **copiabili/modificabili** direttamente dall’interfaccia.

---

## 3. Calendario delle Prenotazioni

### 3.1 Modalità di visualizzazione
L’utente deve poter alternare tra quattro viste tramite pulsanti/tabs:
- **Giornaliera**: elenco orario delle prenotazioni di un singolo giorno.
- **Settimanale**: griglia 7 giorni × fasce orarie (es. 8:00–20:00).
- **Mensile**: calendario classico con indicatori visivi (es. pallini colorati) per carico giornaliero.
- **Annuale**: vista per mese, con indicatori di saturazione (basso/medio/alto).

### 3.2 Interazioni comuni
- Clic su una data/ora apre il dettaglio delle prenotazioni di quel momento.
- Le prenotazioni sono differenziate per tipo:
  - Colore A: Escursioni
  - Colore B: Gommoni
- È possibile creare una nuova prenotazione direttamente dal calendario (es. pulsante “+” su una cella o drag & drop).

### 3.3 Dati visualizzati
Per ogni prenotazione mostrare almeno:
- Nome cliente
- Tipo servizio
- Numero partecipanti (o note brevi)
- Stato (con codifica colore)

---

## 4. Regole di Business Chiave

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento d’identità richiesto |
| Gommone ≥ 100 CV | Documento + patente nautica obbligatori |
| Escursione | Solo documento d’identità richiesto |
| Prenotazione multi-data | Prezzo = somma dei costi per ogni data |
| Modifica “Prezzo reale” | Non sovrascrivere dopo intervento manuale |

---

## 5. Note per l’Agente AI

- Priorità: chiarezza funzionale > stile grafico.
- Assumere che l’utente sia staff interno (non cliente finale).
- Tutti i campi sensibili (CF, documento, ecc.) devono rispettare normative GDPR (visualizzazione controllata, non esportazione non autorizzata).
- Il prototipo deve supportare flussi in cui un cliente esistente viene cercato e i suoi dati popolati automaticamente nel form di prenotazione.